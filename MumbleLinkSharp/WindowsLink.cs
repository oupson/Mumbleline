using Mumbleline.MumbleLink.Data;
using Mumbleline.MumbleLink.FFI.Windows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mumbleline.MumbleLink
{
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

        public override LinkInformations ReadInfos()
        {
            var mem = file.Read();

            return mem.ToInfos();
        }

        public override void WriteInfos(LinkInformations infos)
        {
            mut.WaitOne();
            if (infos.UiTick == 0)
                lastMem.Tick();
            lastMem.UpdateFrom(infos);
            file.Write(ref lastMem);
            mut.ReleaseMutex();
        }

        public async Task TickTaskFunction(CancellationToken token)
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
            tokenSource.Cancel();
            tickTask.Wait();
            tokenSource = null;
            tickTask = null;
        }
    }
}
