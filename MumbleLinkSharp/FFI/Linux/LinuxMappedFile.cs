using System;
using System.Runtime.InteropServices;

namespace Mumbleline.MumbleLink.FFI.Linux
{
    class LinuxMappedFile : IMumbleMappedFile<LinuxLinkedMem>, IDisposable
    {

        [DllImport("libc")]
        public static unsafe extern uint getuid();

        [DllImport("libc")]
        public static unsafe extern IntPtr mmap(void* addr, UIntPtr length, int prot, int flags, int fd, IntPtr offset);

        [DllImport("libc")]
        public static unsafe extern string strerror(int code);


        [DllImport("libc")]
        public static unsafe extern IntPtr memcpy(IntPtr dest, IntPtr src, UIntPtr length);

        [DllImport("librt.so", SetLastError = true)]
        public static unsafe extern int shm_open(string name, int oflag, UInt32 mode);

        [DllImport("librt.so")]
        public static unsafe extern int shm_unlink(string name);

        private readonly string linkName;
        private readonly IntPtr  mappedFile;

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
            memcpy(mappedFile, (IntPtr)(mem), (UIntPtr)Marshal.SizeOf<LinuxLinkedMem>());
        }


        public void Tick()
        {
            var tick = Marshal.ReadInt32(mappedFile, 4);
            Marshal.WriteInt32(mappedFile, 4, tick + 1);
        }

        public void Dispose()
        {
            // TODO UNMAP
            shm_unlink(linkName);
        }

        public LinuxMappedFile()
        {
            unsafe
            {
                // TODO DISPOSE
                linkName = string.Format("/MumbleLink.{0}", getuid());
                int shmfd = shm_open(linkName, 0x0002, 0000400 | 0000200);
                if (shmfd < 0)
                {
                    throw new Exception(string.Format("Something went wrong, shmfd : {0}, error code : {1}", shmfd, strerror(Marshal.GetLastWin32Error()))); // TODO
                }

                mappedFile = mmap(null, (UIntPtr)Marshal.SizeOf<LinuxLinkedMem>(), 0x1 | 0x2, 0x01, shmfd, (IntPtr)0); // TODO CHECK ERROR
            }
        }
    }
}