using Celeste.Mod;
using MumbleLinkSharp;
using MumbleLinkSharp.Data;
using System;

namespace Mumbleline
{
    public class MumbleModule : EverestModule
    {
        // Only one alive module instance can exist at any given time.
        public static MumbleModule Instance;
        private static readonly string TAG = "MumbleModule";
        private bool isMumbleLoaded = false;
        private bool wasLoadCalled = false;

        private MumbleLink mumbler = null;

        public override Type SettingsType => typeof(MumblineSettings);
        public static MumblineSettings Settings => (MumblineSettings)Instance._Settings;

        public MumbleModule()
        {
            Instance = this;
        }

        // Set up any hooks, event handlers and your mod in general here.
        // Load runs before Celeste itself has initialized properly.
        public override void Load()
        {
            wasLoadCalled = true;
            if (Settings.Link)
                LoadMumbline();
        }

        // Optional, initialize anything after Celeste has initialized itself properly.
        public override void Initialize()
        {
        }

        // Optional, do anything requiring either the Celeste or mod content here.
        public override void LoadContent(bool firstLoad)
        {
        }

        // Unload the entirety of your mod's content. Free up any native resources.
        public override void Unload()
        {
            UnloadMumbline();
        }

        private Celeste.LevelData MapData_StartLevel(On.Celeste.MapData.orig_StartLevel orig, Celeste.MapData self)
        {
            mumbler.WriteInfos(new LinkInformations
            {
                Context = string.Format("CELESTE/{0}", self.Data.Name)
            });
            return orig(self);
        }

        private void Player_Update(On.Celeste.Player.orig_Update orig, Celeste.Player self)
        {
            try
            {
                var exactPos = self.ExactPosition;
                Vector3D facingVector;
                if (Settings.PositionnalAudio)
                    facingVector = new Vector3D(0, 0, 1);
                else
                    facingVector = new Vector3D(0, 1, 0);
                var pos = new Vector3D(exactPos.X / 16, exactPos.Y / 16, 0);
                mumbler.WriteInfos(new LinkInformations
                {
                    AvatarPosition = pos,
                    CameraPosition = pos,
                    AvatarFront = facingVector,
                    CameraFront = facingVector
                });
            }
            catch (NullReferenceException e)
            {
                Logger.Log(TAG, e.ToString());
            }

            orig(self);
        }

        private System.Collections.IEnumerator OuiChapterSelect_Enter(On.Celeste.OuiChapterSelect.orig_Enter orig, Celeste.OuiChapterSelect self, Celeste.Oui from)
        {
            mumbler.WriteInfos(new LinkInformations
            {
                Context = string.Format("CELESTE/ONMAP")
            });
            return orig(self, from);
        }

        public void LoadMumbline()
        {
            if (wasLoadCalled && !isMumbleLoaded)
            {
                Logger.Log(TAG, "Loading Mumbleline");
                mumbler = MumbleLink.GetNewInstance();

                mumbler.WriteInfos(new LinkInformations
                {
                    UiVersion = 1,
                    UiTick = 1,
                    Name = "Mumbleline",
                    Description = "A mumble link mod for Celeste",
                    AvatarTop = new Vector3D(0, 1, 0),
                    CameraTop = new Vector3D(0, 1, 0),
                    Context = "NotConnected"
                });

                On.Celeste.Player.Update += Player_Update;
                On.Celeste.MapData.StartLevel += MapData_StartLevel;
                On.Celeste.OuiChapterSelect.Enter += OuiChapterSelect_Enter;
                isMumbleLoaded = true;
            }
        }

        public void UnloadMumbline()
        {
            if (wasLoadCalled && isMumbleLoaded)
            {
                Logger.Log(TAG, "Unloading Mumbleline");
                On.Celeste.Player.Update -= Player_Update;
                On.Celeste.MapData.StartLevel -= MapData_StartLevel;
                On.Celeste.OuiChapterSelect.Enter -= OuiChapterSelect_Enter;
                mumbler.Dispose();
                mumbler = null;
                isMumbleLoaded = false;
            }
        }
    }
}