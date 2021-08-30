using MumbleLinkSharp.Data;
using System;

namespace MumbleLinkSharp
{
    /// <summary>
    /// Provide a way to speak to the mumble link plugin.
    /// </summary>
    public abstract class MumbleLink : IDisposable
    {
        /// <summary>
        /// Read the current informations from the shared memory.
        /// </summary>
        /// <returns>The informations shared with mumble.</returns>
        public abstract LinkInformations ReadInfos();

        /// <summary>
        /// Write informations to mumble link's shared memory.
        /// </summary>
        /// <param name="infos">The informations to write. If a member of this class is null, the current member in the shared memory will not be overwritten.</param>
        public abstract void WriteInfos(LinkInformations infos);
        private static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }


        /// <summary>
        /// Get a windows or linux instance of a mumblelink.
        /// </summary>
        /// <returns>An instance of mumblelink.</returns>
        /// <see cref="WindowsLink"/>
        /// <see cref="LinuxLink"/>
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

        public abstract void Dispose();
    }
}
