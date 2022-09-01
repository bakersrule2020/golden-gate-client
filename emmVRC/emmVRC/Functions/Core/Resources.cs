using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnhollowerRuntimeLib;
using emmVRC.Objects.ModuleBases;
using emmVRCLoader;
using emmVRC.Utils;

namespace emmVRC.Functions.Core
{
    [Priority(0)]
    public class Resources : MelonLoaderEvents
    {
        // Main AssetBundle for emmVRC's Resources
        private static string resourcePath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Resources");

        // Gradient texture and material, for use in custom colors for the loading screen
        // Icons, for use in notifications, the logo, nameplate textures, buttons and the HUDs
        public static Sprite offlineSprite;
        public static Sprite onlineSprite;
        public static Sprite owonlineSprite; // April fools~
        public static Sprite alertSprite;
        public static Sprite errorSprite;
        public static Sprite messageSprite;
        public static Sprite alarmSprite;
        public static Sprite rpSprite;
        public static Sprite crownSprite;
        public static Sprite HUD_Base;
        public static Sprite HUD_Minimized;
        public static Sprite emmHUDLogo;
        public static Sprite TabIcon;
        public static Sprite authorSprite;
        public static Sprite lensOn;
        public static Sprite lensOff;
        public static Sprite zoomIn;
        public static Sprite zoomOut;

        public static Sprite WorldIcon;
        public static Sprite WorldHistoryIcon;
        public static Sprite PlayerIcon;
        public static Sprite PlayerHistoryIcon;
        public static Sprite ProgramsIcon;
        public static Sprite SettingsIcon;
        public static Sprite AlarmClockIcon;
        public static Sprite TeamIcon;
        public static Sprite SupporterIcon;
        public static Sprite ChangelogIcon;
        public static Sprite CheckMarkIcon;

        // Quick Method for making adding sprites easier
        private Sprite LoadSprite(string sprite)
        {
            return $"{resourcePath}/{sprite}".LoadSpriteFromDisk();
        }

        public override void OnUiManagerInit()
        {
            emmVRCLoader.Logger.LogDebug("Initializing resources...");
            LoadResources();
        }

        // Main function for loading in all the resources from the web and locally
        public void LoadResources()
        {

            // Made loading much simpler. If issues are found add yield return before each sprite.
            offlineSprite = LoadSprite("Offline.png");
            onlineSprite = LoadSprite("Online.png");
            owonlineSprite = LoadSprite("OwOnline.png");

            alertSprite = LoadSprite("Alert.png");
            errorSprite = LoadSprite("Error.png");
            messageSprite = LoadSprite("Message.png");
            alarmSprite = LoadSprite("Alarm.png");
            rpSprite = LoadSprite("RP.png");

            lensOn = LoadSprite("LensOn.png");
            lensOff = LoadSprite("LensOff.png");
            zoomIn = LoadSprite("ZoomIn.png");
            zoomOut = LoadSprite("ZoomOut.png");

            crownSprite = LoadSprite("Crown.png");
            authorSprite = LoadSprite("Author.png");

            HUD_Base = LoadSprite("UIMaximized.png");
            HUD_Minimized = LoadSprite("UIMinimized.png");
            emmHUDLogo = LoadSprite("emmSimplifedLogo.png");

            TabIcon = LoadSprite("TabIcon.png");

            WorldIcon = LoadSprite("Globe.png");
            WorldHistoryIcon = LoadSprite("GlobeHistory.png");
            PlayerIcon = LoadSprite("Player.png");
            PlayerHistoryIcon = LoadSprite("PlayerHistory.png");
            SupporterIcon = LoadSprite("Heart.png");
            ProgramsIcon = LoadSprite("Programs.png");
            SettingsIcon = LoadSprite("Settings.png");
            AlarmClockIcon = LoadSprite("AlarmClock.png");
            TeamIcon = LoadSprite("RoseIcon.png");
            ChangelogIcon = LoadSprite("Changelog.png");

            CheckMarkIcon = LoadSprite("Checkmark.png");
        }
    }
}