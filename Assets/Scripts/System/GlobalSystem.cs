using UnityEngine;
using RoomUDPSystem;

public static class GlobalSystem
{
    public static bool m_IsPause { get; private set; }
    public static void SetPause(bool pause_) { m_IsPause = pause_; }

    public enum MatchState { Non, Tutorial, Local, Multi }
    public static MatchState m_MatchState { get; private set; }
    public static void SetMatchState(MatchState state_) { m_MatchState = state_; }

    public static RoomUDP.RoomState m_RoomState { get { return RoomUDP.State; } }
}

namespace Name
{
    public static class Scene
    {
        public const string Message = "Message";
        public const string Title = "Title";
        public const string Game = "Game";
        //public const string GameMode = "GameMode";//チュートリアルローカルマルチを選択するシーン
        public const string Result = "Result";
    }

    public static class SortingLayer
    {
        public const int Default = 0;
        public const int UI = 1;
        public const int UI_Effect = 2;
    }

    public static class Layer
    {
        public const int Default = 0;
        public const int TransparentFX = 1;
        public const int IgnoreRaycast = 2;
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

    public static class FilePath
    {
#if UNITY_EDITOR
        public static readonly string AppFilePath = Application.dataPath + "/Save/";
#else
        public static readonly string AppFilePath = Application.dataPath + "/../";
#endif

#if UNITY_EDITOR
        public static readonly string FilePath_Deck = AppFilePath + "/Decks";
#else
        public static readonly string FilePath_Deck = Application.dataPath + "Decks";
#endif

        public static readonly string FilePath_Setting = AppFilePath + "Settings";
        public static readonly string FilePath_ScreenShot = AppFilePath + "ScreenShots";
    }
}