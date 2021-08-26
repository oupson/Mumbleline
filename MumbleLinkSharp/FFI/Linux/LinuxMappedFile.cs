using System;
using System.Runtime.InteropServices;

namespace Mumbleline.MumbleLink.FFI.Linux
{
    class LinuxMappedFile : MumbleMappedFile<LinuxLinkedMem>, IDisposable
    {

        [DllImport("libc")]
        public static unsafe extern uint getuid();

        [DllImport("libc")]
        public static unsafe extern IntPtr mmap(void* addr, UIntPtr length, int prot, int flags, int fd, IntPtr offset);

        [DllImport("libc")]
        public static unsafe extern string strerror(int code);

        [DllImport("librt.so", SetLastError = true)]
        public static unsafe extern int shm_open(string name, int oflag, UInt32 mode);

        [DllImport("librt.so")]
        public static unsafe extern int shm_unlink(string name);

        private string linkName;
        private IntPtr mappedFile;

        public LinuxLinkedMem Read()
        {
            return Marshal.PtrToStructure<LinuxLinkedMem>(mappedFile);
        }

        public void Write(LinuxLinkedMem mem)
        {
            Marshal.StructureToPtr(mem, mappedFile, false);
        }

        public void Tick()
        {
            var tick = Marshal.ReadInt32(mappedFile, 4);
            Marshal.WriteInt32(mappedFile, 4, tick + 1);
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing ...");
            shm_unlink(linkName);
        }

        public LinuxMappedFile()
        {
            unsafe
            {
                // TODO DISPOSE
                linkName = string.Format("/MumbleLink.{0}", getuid());
                Console.WriteLine(linkName);
                int shmfd = shm_open(linkName, 0x0002, 0000400 | 0000200);
                if (shmfd < 0)
                {
                    throw new Exception(string.Format("Something went wrong, shmfd : {0}, error code : {1}", shmfd, strerror(Marshal.GetLastWin32Error()))); // TODO
                }

                mappedFile = mmap(null, (UIntPtr)Marshal.SizeOf<LinuxLinkedMem>(), 0x1 | 0x2, 0x01, shmfd, (IntPtr)0);

                Console.WriteLine(mappedFile);
            }
        }
    }
}
