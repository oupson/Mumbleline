using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace MumbleLinkSharp.FFI.Windows
{
    class WindowsMappedFile : IMumbleMappedFile<WindowsLinkedMem>, IDisposable
    {
        private readonly MemoryMappedFile file;

        public WindowsMappedFile(string linkName = "MumbleLink")
        {
            file = MemoryMappedFile.CreateOrOpen(linkName, Marshal.SizeOf(typeof(WindowsLinkedMem)));
        }

        public WindowsLinkedMem Read()
        {
            WindowsLinkedMem mem;
            using (var accessor = file.CreateViewAccessor())
            {
                accessor.Read(0, out mem);
            }

            return mem;
        }

        public void Write(ref WindowsLinkedMem mem)
        {
            using (var accessor = file.CreateViewAccessor())
            {
                accessor.Write(0, ref mem);
            }
        }

        public void Tick()
        {
            using (var accessor = file.CreateViewAccessor())
            {
                var t = accessor.ReadUInt32(4);
                accessor.Write(4, t + 1);
            }
        }

        public void Dispose()
        {
            file.Dispose();
        }
    }
}
