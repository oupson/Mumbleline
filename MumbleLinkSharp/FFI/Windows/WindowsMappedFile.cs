using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Mumbleline.MumbleLink.FFI.Windows
{
    class WindowsMappedFile : MumbleMappedFile<WindowsLinkedMem>
    {
        private readonly MemoryMappedFile file;

        public WindowsMappedFile(string linkName = "MumbleLink")
        {
            file = MemoryMappedFile.CreateOrOpen(linkName, Marshal.SizeOf(typeof(LinkedMem)));
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

        public void Write(WindowsLinkedMem mem)
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
    }
}
