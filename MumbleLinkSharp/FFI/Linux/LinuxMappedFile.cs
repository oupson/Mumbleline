using System;
using System.Runtime.InteropServices;

namespace MumbleLinkSharp.FFI.Linux
{
    class LinuxMappedFile : IMumbleMappedFile<LinuxLinkedMem>, IDisposable
    {
        private readonly string linkName;
        private IntPtr mappedFile;

        public LinuxLinkedMem Read()
        {
            return Marshal.PtrToStructure<LinuxLinkedMem>(mappedFile);
        }

        public void Write(ref LinuxLinkedMem mem)
        {
            Marshal.StructureToPtr(mem, mappedFile, false); // BUGGY WITH CELESTE ON LINUX
        }

        public unsafe void Write(LinuxLinkedMem* mem)
        {
            Bindings.memcpy(mappedFile, (IntPtr)(mem), (UIntPtr)Marshal.SizeOf<LinuxLinkedMem>());
        }


        public void Tick()
        {
            var tick = Marshal.ReadInt32(mappedFile, 4);
            Marshal.WriteInt32(mappedFile, 4, tick + 1);
        }

        public void Dispose()
        {
            // TODO Check error
            unsafe
            {
                Bindings.shm_unlink(linkName);
                Bindings.munmap(mappedFile.ToPointer(), (UIntPtr)Marshal.SizeOf<LinuxLinkedMem>());
            }
            mappedFile = (IntPtr)0;
        }

        public LinuxMappedFile()
        {
            unsafe
            {
                linkName = string.Format("/MumbleLink.{0}", Bindings.getuid());
                int shmfd = Bindings.shm_open(linkName, Bindings.O_RDWR | Bindings.O_CREAT, (uint)(Bindings.S_IRUSR | Bindings.S_IWUSR));
                if (shmfd < 0)
                {
                    throw new LinuxFFIException("shm_open", Marshal.GetLastWin32Error());
                }

                if (Bindings.ftruncate(shmfd, (UIntPtr)Marshal.SizeOf<LinuxLinkedMem>()) < 0)
                {
                    throw new LinuxFFIException("ftruncate", Marshal.GetLastWin32Error());
                }

                var resMapping = Bindings.mmap(null, (UIntPtr)Marshal.SizeOf<LinuxLinkedMem>(), Bindings.PROT_READ | Bindings.PROT_WRITE, Bindings.MAP_SHARED, shmfd, (IntPtr)0);
                if ((long)resMapping < 0)
                {
                    throw new LinuxFFIException("mmap", Marshal.GetLastWin32Error());
                }

                mappedFile = resMapping;
            }
        }
    }
}