using System;
using System.Runtime.InteropServices;

namespace Mumbleline.MumbleLink.FFI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct LinkedMem
    {
        public UInt32 uiVersion;
        public UInt32 uiTick;
        public fixed float fAvatarPosition[3];
        public fixed float fAvatarFront[3];
        public fixed float fAvatarTop[3];

        public fixed char name[256];

        public fixed float fCameraPosition[3];

        public fixed float fCameraFront[3];

        public fixed float fCameraTop[3];

        public fixed char identity[256];

        public UInt32 context_len;
        public fixed byte context[256];

        public fixed char description[2048];
    }
}
