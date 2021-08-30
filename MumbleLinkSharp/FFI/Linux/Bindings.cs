using System;
using System.Runtime.InteropServices;

namespace MumbleLinkSharp.FFI.Linux
{
    class Bindings
    {
        [DllImport("libc")]
        public static unsafe extern uint getuid();

        [DllImport("libc", SetLastError = true)]
        public static unsafe extern IntPtr mmap(void* addr, UIntPtr length, int prot, int flags, int fd, IntPtr offset);

        [DllImport("libc", SetLastError = true)]
        public static unsafe extern int munmap(void* addr, UIntPtr length);

        [DllImport("libc", SetLastError = true)]
        public static unsafe extern int __xpg_strerror_r(int code, char* buf, UIntPtr buflen);

        [DllImport("libc")]
        public static unsafe extern IntPtr memcpy(IntPtr dest, IntPtr src, UIntPtr length);

        [DllImport("librt.so", SetLastError = true)]
        public static unsafe extern int shm_open(string name, int oflag, UInt32 mode);

        [DllImport("librt.so", SetLastError = true)]
        public static unsafe extern int shm_unlink(string name);

        [DllImport("libc", SetLastError = true)]
        public static unsafe extern int ftruncate(int fd, UIntPtr length);

        public static int O_RDWR = 02;
        public static int O_CREAT = 0100;
        public static int S_IRUSR = 0400;
        public static int S_IWUSR = 0200;

        public static int PROT_READ = 0x1;
        public static int PROT_WRITE = 0x2;
        public static int MAP_SHARED = 0x01;

        public static string GetErrorCodeDescription(int errorCode)
        {
            unsafe
            {
                char* ptr = (char*)Marshal.AllocHGlobal(256).ToPointer();
                var resCode = __xpg_strerror_r(errorCode, ptr, (UIntPtr)256);
                var res = Marshal.PtrToStringAuto((IntPtr)ptr);
                Marshal.FreeHGlobal((IntPtr)ptr);
                return res;
            }
        }
    }
}