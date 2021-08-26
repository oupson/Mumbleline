using Mumbleline.MumbleLink.Data;
using Mumbleline.MumbleLink.FFI.Linux;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mumbleline.MumbleLink
{
    public class LinuxLink : MumbleLink, IDisposable
    {
        private LinuxMappedFile file;
        private LinuxLinkedMem lastMem;

        private Task tickTask;
        private readonly Mutex mut = new Mutex();
        private CancellationTokenSource tokenSource;

        public LinuxLink()
        {
            file = new LinuxMappedFile();

            tokenSource = new CancellationTokenSource();

            lastMem = new LinuxLinkedMem();

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
            unsafe // THIS FIX CELESTE BUG
            {
                fixed (LinuxLinkedMem* memFoo = &lastMem)
                {
                    file.Write(memFoo);
                }
            }
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

        public void Dispose()
        {
            tokenSource.Cancel();
            tickTask.Wait();
            tokenSource = null;
            tickTask = null;
        }
    }
}
