using Mumbleline.MumbleLink.Data;
using System;

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
            else
            {
                return new WindowsLink();
            }
        }
    }
}
