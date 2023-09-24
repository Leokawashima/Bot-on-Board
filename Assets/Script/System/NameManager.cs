using UnityEngine;

namespace Name
{
    public static class Scene
    {
        public const string Message = "Message";
        public const string Title = "Title";
        public const string Game = "Game";
    }

    public static class Tag
    {
        public const string Normal_Map = "Normal_Map";
    }

    public static class SortingLayer
    {
        public const int Default = 0;
    }

    public static class Layer
    {
        public const int Default = 0;
        public const int TransparentFX = 1;
        public const int IgnoreRaycaast = 2;
        public const int Water = 4;
        public const int UI = 5;
        public const int PostProcess = 9;
        public const int OutLine = 10;
        public const int Map = 11;
    }

    public static class AudioMixer
    {
        public static class Volume
        {
            public const string Master = "Volume_Master";
            public const string BGM = "Volume_BGM";
            public const string SE = "Volume_SE";
            public const string UI = "Volume_UI";
        }
    }

    public static class Setting
    {
#if UNITY_EDITOR
        public static readonly string AppFilePath = Application.dataPath + "/";
#else
        public static readonly string AppFilePath = Application.dataPath + "/../";
#endif
        public static readonly string SettingFilePath = AppFilePath + "Settings";
        public static readonly string ScreenShotFilePath = AppFilePath + "ScreenShots";
    }
}