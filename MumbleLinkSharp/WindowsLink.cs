using MumbleLinkSharp.Data;
using MumbleLinkSharp.FFI.Windows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MumbleLinkSharp
{
    /// <summary>
    /// Provide a way to speak to the mumble link plugin in windows.
    /// As linux and windows mapped file are different, and the fact that .net mappedfile are not fully supported on linux, different classes are required.
    /// However, you should use MumbleLink instead of this class, as it provide an abstraction over operating systems.
    /// </summary>
    public class WindowsLink : MumbleLink, IDisposable
    {
        private WindowsMappedFile file;
        private WindowsLinkedMem lastMem;

        private Task tickTask;
        private readonly Mutex mut = new Mutex();
        private CancellationTokenSource tokenSource;

        public WindowsLink()
        {
            file = new WindowsMappedFile();

            tokenSource = new CancellationTokenSource();

            tickTask = TickTaskFunction(tokenSource.Token);
        }

        /// <summary>
        /// Read the current informations from the shared memory.
        /// </summary>
        /// <returns>The informations shared with mumble.</returns>
        public override LinkInformations ReadInfos()
        {
            var mem = file.Read();

            return mem.ToInfos();
        }

        /// <summary>
        /// Write informations to mumble link's shared memory.
        /// </summary>
        /// <param name="infos">The informations to write. If a member of this class is null, the current member in the shared memory will not be overwritten.</param>
        public override void WriteInfos(LinkInformations infos)
        {
            mut.WaitOne();
            if (infos.UiTick == 0)
                lastMem.Tick();
            lastMem.UpdateFrom(infos);
            file.Write(ref lastMem);
            mut.ReleaseMutex();
        }

        private async Task TickTaskFunction(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                mut.WaitOne();
                lastMem.Tick();
                file.Tick();
                mut.ReleaseMutex();
                await Task.Delay(1000 / 50);
            }
        }

        public override void Dispose()
        {
            mut.WaitOne();
            tokenSource.Cancel();
            tickTask.Wait();
            tokenSource = null;
            tickTask = null;
            file.Dispose();
            file = null;
            mut.ReleaseMutex();
        }
    }
}