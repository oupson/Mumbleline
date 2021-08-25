using Mumbleline.MumbleLink.Data;
using Mumbleline.MumbleLink.FFI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mumbleline.MumbleLink
{
    public class MumbleLink : IDisposable
    {
        private MumbleMappedFile file;
        private LinkedMem lastMem;

        private Task tickTask;
        private readonly Mutex mut = new Mutex();
        private CancellationTokenSource tokenSource;

        public MumbleLink()
        {
            if (IsLinux)
            {
                file = new LinuxMappedFile(); // TODO
            }
            else
            {
                file = new WindowsMappedFile();
            }

            tokenSource = new CancellationTokenSource();

            tickTask = TickTaskFunction(tokenSource.Token);
        }

        public LinkInformations ReadInfos()
        {
            var mem = file.Read();

            return LinkInformations.FromLinkedMem(ref mem);
        }

        public void Write(LinkInformations infos)
        {
            mut.WaitOne();
            if (infos.UiTick == 0)
                lastMem.uiTick++;
            lastMem = infos.UpdateMem(lastMem);
            file.Write(lastMem);
            mut.ReleaseMutex();
        }

        public async Task TickTaskFunction(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                mut.WaitOne();
                lastMem.uiTick++;
                file.Tick();
                mut.ReleaseMutex();
                await Task.Delay(1000 / 50);
            }
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            tickTask.Wait();
            tokenSource = null;
            tickTask = null;
        }
        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
    }
}
