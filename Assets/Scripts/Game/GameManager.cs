using System;
using UnityEngine;
using Game.GameRule;
using GameMode;

namespace Game
{
    /// <summary>
    /// ゲームの基本的な処理を担うクラス
    /// </summary>
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [field: SerializeField] public GameMode_Template Mode { get; private set; }
        [field: SerializeField] public GameRule_Template Rule { get; private set; }

        [SerializeField] private Material m_skybox;

        public static event Action
            Event_Initialize,
            Event_Finalize;

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private GameDebug m_debug;
        [SerializeField] private GameModeManager.GameMode GameMode = GameModeManager.GameMode.Non;
#endif

        private void Start()
        {
            RenderSettings.skybox = m_skybox;
            SystemInitalize();
        }

        private void SystemInitalize()
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            CameraManager.Singleton.SetFreeLookCamIsMove(false);

            // GameSettingみたいなシーンを作ってゲーム設定代入も読み込みもその単一シーンで賄うほうが話早め　だがとりあえずの処理
#if UNITY_EDITOR
            var _current = GameMode;
            if (GameModeManager.CurrentGameMode != GameModeManager.GameMode.Non)
                _current = GameModeManager.CurrentGameMode;
            
            switch (_current)
#else
            switch (GameModeManager.CurrentGameMode)
#endif
            {
                case GameModeManager.GameMode.Non:
                    // エラー吐くからデータ設定しっかり
                    break;
                case GameModeManager.GameMode.Tutorial:
                    Mode = gameObject.AddComponent<GameModeTutorial>();
                    Rule = gameObject.AddComponent<GameRuleTutorial>();
                    break;
                case GameModeManager.GameMode.Local:
                    Mode = gameObject.AddComponent<GameModeLocal>();
                    Rule = gameObject.AddComponent<GameRuleTurn>();
                    break;
                case GameModeManager.GameMode.Multi:
                    Mode = gameObject.AddComponent<GameModeMulti>();
                    Rule = gameObject.AddComponent<GameRuleTurn>();
                    break;
            }
            Mode.Initialize();

            Event_Initialize?.Invoke();

            Rule.Initialize();
        }
        public void SystemFinalize()
        {
            Event_Finalize?.Invoke();

            Initiate.Fade(Name.Scene.Result, Name.Scene.Game, Color.black, 1.0f);
        }
    }
}