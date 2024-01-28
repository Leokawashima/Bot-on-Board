using System;
using UnityEngine;
using Game.GameRule;

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
        [SerializeField] private GlobalSystem.GameMode GameMode = GlobalSystem.GameMode.Non;
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
            if (GlobalSystem.CurrentGameMode != GlobalSystem.GameMode.Non)
                _current = GlobalSystem.CurrentGameMode;
            
            switch (_current)
#else
            switch (GlobalSystem.CurrentGameMode)
#endif
            {
                case GlobalSystem.GameMode.Non:
                    // エラー吐くからデータ設定しっかり
                    break;
                case GlobalSystem.GameMode.Tutorial:
                    Mode = gameObject.AddComponent<GameModeTutorial>();
                    Rule = gameObject.AddComponent<GameRuleTutorial>();
                    break;
                case GlobalSystem.GameMode.Local:
                    Mode = gameObject.AddComponent<GameModeLocal>();
                    Rule = gameObject.AddComponent<GameRuleTurn>();
                    break;
                case GlobalSystem.GameMode.Multi:
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