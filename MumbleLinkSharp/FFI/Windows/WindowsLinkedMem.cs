using System;
using System.Runtime.InteropServices;
using Mumbleline.MumbleLink.Data;

namespace Mumbleline.MumbleLink.FFI.Windows
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct WindowsLinkedMem : LinkedMem
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

        public void Tick()
        {
            uiTick++;
        }
        public LinkInformations ToInfos()
        {
            unsafe
            {
                string name, identity, context, description;
                fixed (char* namePtr = this.name)
                {
                    name = Marshal.PtrToStringUni((IntPtr)namePtr);
                }

                fixed (char* identityPtr = this.identity)
                {
                    identity = Marshal.PtrToStringUni((IntPtr)identityPtr);
                }


                fixed (byte* contextPtr = this.context)
                {
                    context = Marshal.PtrToStringAnsi((IntPtr)contextPtr, (int)this.context_len);
                }

                fixed (char* descriptionPtr = this.description)
                {
                    description = Marshal.PtrToStringUni((IntPtr)descriptionPtr);
                }

                return new LinkInformations
                {
                    UiVersion = this.uiVersion,
                    UiTick = this.uiTick,
                    AvatarPosition = new Vector3D(this.fAvatarPosition[0], this.fAvatarPosition[1], this.fAvatarPosition[2]),
                    AvatarFront = new Vector3D(this.fAvatarFront[0], this.fAvatarFront[1], this.fAvatarFront[2]),
                    AvatarTop = new Vector3D(this.fAvatarTop[0], this.fAvatarTop[1], this.fAvatarTop[2]),
                    Name = name,
                    CameraPosition = new Vector3D(this.fCameraPosition[0], this.fCameraPosition[1], this.fCameraPosition[2]),
                    CameraFront = new Vector3D(this.fCameraFront[0], this.fCameraFront[1], this.fCameraFront[2]),
                    CameraTop = new Vector3D(this.fCameraTop[0], this.fCameraTop[1], this.fCameraTop[2]),
                    Identity = identity,
                    ContextLen = this.context_len,
                    Context = context,
                    Description = description,
                };

            }
        }

        public LinkedMem UpdateFrom(LinkInformations infos)
        {
            unsafe
            {
                if (infos.UiTick != 0)
                    uiTick = infos.UiTick;

                if (infos.UiVersion != 0)
                    uiVersion = infos.UiVersion;

                if (infos.AvatarPosition != null)
                {
                    fAvatarPosition[0] = infos.AvatarPosition.X;
                    fAvatarPosition[1] = infos.AvatarPosition.Y;
                    fAvatarPosition[2] = infos.AvatarPosition.Z;
                }

                if (infos.AvatarFront != null)
                {
                    fAvatarFront[0] = infos.AvatarFront.X;
                    fAvatarFront[1] = infos.AvatarFront.Y;
                    fAvatarFront[2] = infos.AvatarFront.Z;
                }

                if (infos.AvatarTop != null)
                {
                    fAvatarTop[0] = infos.AvatarTop.X;
                    fAvatarTop[1] = infos.AvatarTop.Y;
                    fAvatarTop[2] = infos.AvatarTop.Z;
                }

                if (infos.Name != null)
                    fixed(char* namePtr = name)
                        CopyWStringInto(infos.Name, namePtr, 256);

                if (infos.CameraPosition != null)
                {
                    fCameraPosition[0] = infos.CameraPosition.X;
                    fCameraPosition[1] = infos.CameraPosition.Y;
                    fCameraPosition[2] = infos.CameraPosition.Z;
                }

                if (infos.CameraFront != null)
                {
                    fCameraFront[0] = infos.CameraFront.X;
                    fCameraFront[1] = infos.CameraFront.Y;
                    fCameraFront[2] = infos.CameraFront.Z;
                }

                if (infos.CameraTop != null)
                {
                    fCameraTop[0] = infos.CameraTop.X;
                    fCameraTop[1] = infos.CameraTop.Y;
                    fCameraTop[2] = infos.CameraTop.Z;
                }

                if (infos.Identity != null)
                    fixed(char* identityPtr = identity)
                        CopyWStringInto(infos.Identity, identityPtr, 256);

                if (infos.Context != null)
                    fixed(byte* contextPtr = context)
                        context_len = CopyAStringInto(infos.Context, contextPtr, 256);

                if (infos.Description != null)
                    fixed(char* descPtr = description)
                        CopyWStringInto(infos.Description, descPtr, 2048);
            }


            return this;
        }

        private unsafe void CopyWStringInto(string str, char* mem, int size)
        {
            var name = (char*)Marshal.StringToHGlobalUni(str);
            var index = 0;
            if (name != null)
            {

                while (index < size - 1 && name[index] != 0)
                {
                    mem[index] = name[index];
                    index++;
                }
            }

            while (index < size)
            {
                mem[index] = (char)0;
                index++;
            }
            Marshal.FreeHGlobal((IntPtr)name);
        }

        private unsafe uint CopyAStringInto(string str, byte* mem, int maxSize)
        {
            var name = (byte*)Marshal.StringToHGlobalAnsi(str);
            var index = 0;
            if (name != null)
            {
                while (index < maxSize - 1 && name[index] != 0)
                {
                    mem[index] = name[index];
                    index++;
                }
            }

            var len = index;

            while (index < maxSize)
            {
                mem[index] = 0;
                index++;
            }
            Marshal.FreeHGlobal((IntPtr)name);

            return (uint)len;
        }
    }
}
