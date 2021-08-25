using Mumbleline.MumbleLink.FFI;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Mumbleline.MumbleLink.Data
{
    public class LinkInformations
    {
        public uint UiVersion { get; set; }
        public uint UiTick { get; set; }
        public Vector3D AvatarPosition { get; set; }
        public Vector3D AvatarFront { get; set; }
        public Vector3D AvatarTop { get; set; }
        public string Name { get; set; }
        public Vector3D CameraPosition { get; set; }
        public Vector3D CameraFront { get; set; }
        public Vector3D CameraTop { get; set; }
        public string Identity { get; set; }
        public uint ContextLen { get; set; }
        public string Context { get; set; }
        public string Description { get; set; }

        public LinkInformations()
        {
        }

        public static LinkInformations FromLinkedMem(ref LinkedMem mem)
        {
            unsafe
            {
                string name, identity, context, description;
                fixed (char* namePtr = mem.name)
                {
                    name = Marshal.PtrToStringUni((IntPtr)namePtr);
                }

                fixed (char* identityPtr = mem.identity)
                {
                    identity = Marshal.PtrToStringUni((IntPtr)identityPtr);
                }


                fixed (byte* contextPtr = mem.context)
                {
                    context = Marshal.PtrToStringAnsi((IntPtr)contextPtr, (int)mem.context_len);
                }

                fixed (char* descriptionPtr = mem.description)
                {
                    description = Marshal.PtrToStringUni((IntPtr)descriptionPtr);
                }

                return new LinkInformations
                {
                    UiVersion = mem.uiVersion,
                    UiTick = mem.uiTick,
                    AvatarPosition = new Vector3D(mem.fAvatarPosition[0], mem.fAvatarPosition[1], mem.fAvatarPosition[2]),
                    AvatarFront = new Vector3D(mem.fAvatarFront[0], mem.fAvatarFront[1], mem.fAvatarFront[2]),
                    AvatarTop = new Vector3D(mem.fAvatarTop[0], mem.fAvatarTop[1], mem.fAvatarTop[2]),
                    Name = name,
                    CameraPosition = new Vector3D(mem.fCameraPosition[0], mem.fCameraPosition[1], mem.fCameraPosition[2]),
                    CameraFront = new Vector3D(mem.fCameraFront[0], mem.fCameraFront[1], mem.fCameraFront[2]),
                    CameraTop = new Vector3D(mem.fCameraTop[0], mem.fCameraTop[1], mem.fCameraTop[2]),
                    Identity = identity,
                    ContextLen = mem.context_len,
                    Context = context,
                    Description = description,
                };

            }
        }

        public LinkedMem UpdateMem(LinkedMem mem)
        {
            unsafe
            {
                if (UiTick != 0)
                    mem.uiTick = UiTick;

                if (UiVersion != 0)
                    mem.uiVersion = UiVersion;

                if (AvatarPosition != null)
                {
                    mem.fAvatarPosition[0] = AvatarPosition.X;
                    mem.fAvatarPosition[1] = AvatarPosition.Y;
                    mem.fAvatarPosition[2] = AvatarPosition.Z;
                }

                if (AvatarFront != null)
                {
                    mem.fAvatarFront[0] = AvatarFront.X;
                    mem.fAvatarFront[1] = AvatarFront.Y;
                    mem.fAvatarFront[2] = AvatarFront.Z;
                }

                if (AvatarTop != null)
                {
                    mem.fAvatarTop[0] = AvatarTop.X;
                    mem.fAvatarTop[1] = AvatarTop.Y;
                    mem.fAvatarTop[2] = AvatarTop.Z;
                }

                if (Name != null)
                    CopyWStringInto(Name, mem.name, 256);

                if (CameraPosition != null)
                {
                    mem.fCameraPosition[0] = CameraPosition.X;
                    mem.fCameraPosition[1] = CameraPosition.Y;
                    mem.fCameraPosition[2] = CameraPosition.Z;
                }

                if (CameraFront != null)
                {
                    mem.fCameraFront[0] = CameraFront.X;
                    mem.fCameraFront[1] = CameraFront.Y;
                    mem.fCameraFront[2] = CameraFront.Z;
                }

                if (CameraTop != null)
                {
                    mem.fCameraTop[0] = CameraTop.X;
                    mem.fCameraTop[1] = CameraTop.Y;
                    mem.fCameraTop[2] = CameraTop.Z;
                }

                if (Identity != null)
                    CopyWStringInto(Identity, mem.identity, 256);

                if (Context != null)
                    mem.context_len = CopyAStringInto(Context, mem.context, 256);

                if (Description != null)
                    CopyWStringInto(Description, mem.description, 2048);
            }


            return mem;
        }

        public LinkedMem ToLinkedMem()
        {
            LinkedMem mem = new LinkedMem();
            return UpdateMem(mem);
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.GetType().Name);
            sb.Append("(");
            foreach (System.Reflection.PropertyInfo property in this.GetType().GetProperties())
            {
                sb.Append(property.Name);
                sb.Append(" : ");
                if (property.GetIndexParameters().Length > 0)
                {
                    sb.Append("Indexed Property cannot be used");
                }
                else
                {
                    sb.Append(property.GetValue(this, null));
                }

                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");

            return sb.ToString();
        }
    }
}