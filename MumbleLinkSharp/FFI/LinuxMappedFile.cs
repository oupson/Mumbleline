using System;
using System.Runtime.InteropServices;

namespace Mumbleline.MumbleLink.FFI
{
    class LinuxMappedFile : MumbleMappedFile
    {
        [DllImport("libc")]
        public static extern uint getuid();

        public LinuxMappedFile()
        {
            var linkname = string.Format("/MumbleLink.{0}", getuid());
        }

        public LinkedMem Read()
        {
            throw new NotImplementedException();
        }

        public void Write(LinkedMem mem)
        {
            throw new NotImplementedException();
        }

        public void Tick()
        {
            throw new NotImplementedException();
        }
    }
}
