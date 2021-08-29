using Celeste.Mod;
using System;

namespace Mumbleline
{
    public class MumblineSettings : EverestModuleSettings
    {
        private bool _link = true;
        public bool Link
        {
            get
            {
                return _link;
            }
            set
            {
                Console.WriteLine("Link {0}", value);
                if (value)
                    MumbleModule.Instance.LoadMumbline();
                else
                    MumbleModule.Instance.UnloadMumbline();
                _link = value;
            }
        }

        public bool PositionnalAudio { get; set; } = true;
    }
}
