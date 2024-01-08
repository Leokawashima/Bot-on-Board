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
        [SerializeField] private GameDebug m_debug;
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

            Mode = gameObject.AddComponent<GameModeLocal>();
            Mode.Initialize();

            Event_Initialize?.Invoke();

            Rule = gameObject.AddComponent<GameRuleTurn>();
            Rule.Initialize();
        }
        public void SystemFinalize()
        {
            Event_Finalize?.Invoke();

            Initiate.Fade(Name.Scene.Result, Name.Scene.Game, Color.black, 1.0f);
        }
    }
}