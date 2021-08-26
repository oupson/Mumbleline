using Mumbleline.MumbleLink.Data;
using Mumbleline.MumbleLink.FFI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mumbleline.MumbleLink
{
    public abstract class MumbleLink
    {
        public abstract LinkInformations ReadInfos();
        public abstract void WriteInfos(LinkInformations infos);

        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public static MumbleLink GetNewInstance()
        {
            if (IsLinux)
            {
                return new LinuxLink();
            }
            return new WindowsLink();
        }
    }
}
