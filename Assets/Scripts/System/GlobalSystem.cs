using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;
using Deck;
using UnityEngine.InputSystem;

public static class GlobalSystem
{
    public static bool IsPause { get; private set; }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetPause(bool pause_) { IsPause = pause_; }

    public static GameModeState GameMode { get; private set; }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetGameMode(GameModeState state_) { GameMode = state_; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnRuntimeInitialize()
    {
        SetPause(false);
        SetGameMode(GameModeState.Non);
        IndexPlayerFirst = -1;
        DeckPlayerFirst = null;
        IndexPlayerSecond = -1;
        DeckPlayerSecond = null;
        
        var _input = new InputActionMapSettings();
        _input.UI.Esc.started += (InputAction.CallbackContext context_) =>
        {
            Application.Quit();
        };
        _input.Enable();

        SceneManager.LoadScene(Name.Scene.System, LoadSceneMode.Additive);
    }
    public static int
        IndexPlayerFirst,
        IndexPlayerSecond;
    public static DeckData
        DeckPlayerFirst,
        DeckPlayerSecond;
}

namespace Name
{
    public static class Scene
    {
        public const string System = "System";
        public const string Message = "Message";
        public const string Title = "Title";
        public const string Deck = "Deck";
        public const string GameMode = "GameMode";
        public const string Room = "Room";
        public const string Game = "Game";
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