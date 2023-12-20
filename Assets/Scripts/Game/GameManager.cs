using System;
using System.Collections;
using UnityEngine;
using Map;
using Bot;
using Player;

namespace Game
{
    /// <summary>
    /// ゲームの基本的な処理を担うクラス
    /// </summary>
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [SerializeField] DiceSystem m_DiceSystem;
        [SerializeField] CutInSystem m_cutInSystem;
        [SerializeField] GameMode_Template m_gameMode;

        [field: SerializeField] public int TurnElapsed { get; private set; } = 1;
        [field: SerializeField] public int ProgressPlayerIndex { get; private set; } = 0;
        [field: SerializeField] public int TurnSuddenDeath { get; private set; } = 30;
        [field: SerializeField] public int TurnForceFinish { get; private set; } = 50;

        public static event Action
            Event_Initialize,
            Event_Turn_Initialize,
            Event_Turn_Place,
            Event_Turn_TurnEnd,
            Event_Turn_AIAction,
            Event_Turn_GameSet,
            Event_Turn_Finalize,
            Event_Finalize;

#if UNITY_EDITOR
        [SerializeField] private GameDebug m_debug;
#endif

        #region EventSubscribe
        void OnEnable()
        {
            MapManager.Event_MapCreated += OnMapCreated;
            GUIManager.Event_TurnInitializeCutIn += OnInitializeCutIn;
            PlayerUIManager.Event_ButtonTurnEnd += OnButton_TurnEnd;
            GUIManager.Event_AnimGameSet += SystemFinalize;
            BotManager.Event_BotsActioned += TurnFinalize;
        }
        void OnDisable()
        {
            MapManager.Event_MapCreated -= OnMapCreated;
            GUIManager.Event_TurnInitializeCutIn -= OnInitializeCutIn;
            PlayerUIManager.Event_ButtonTurnEnd -= OnButton_TurnEnd;
            GUIManager.Event_AnimGameSet -= SystemFinalize;
            BotManager.Event_BotsActioned -= TurnFinalize;
        }
        #endregion EventSubscribe

        void Start()
        {
            SystemInitalize();
        }

        void SystemInitalize()
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond + DateTime.Now.Second);
            CameraManager.Singleton.SetFreeLookCamIsMove(false);

            m_gameMode = gameObject.AddComponent<GameModeLocal>();
            m_gameMode.Initialize();

            Event_Initialize?.Invoke();
        }
        void SystemFinalize()
        {
            Event_Finalize?.Invoke();

            Initiate.Fade(Name.Scene.Result, Color.black, 1.0f);
        }

        void TurnInitialize()
        {
            ProgressPlayerIndex = 0;

            Event_Turn_Initialize?.Invoke();
        }
        void TurnPlace()
        {
            Event_Turn_Place?.Invoke();
        }
        void TurnAIAction()
        {
            Event_Turn_AIAction?.Invoke();
        }
        void TurnFinalize()
        {
            Event_Turn_Finalize?.Invoke();

            if (BotManager.Singleton.CheckBotDead())
            {
                TurnGameSet();
            }

            if (TurnElapsed < TurnForceFinish)
            {
                TurnElapsed++;

                TurnInitialize();
            }
            else
            {
                TurnGameSet();
            }
        }
        void TurnGameSet()
        {
            Event_Turn_GameSet?.Invoke();
        }

        void OnMapCreated()
        {
            BotManager.Singleton.Initialize();

            TurnInitialize();
        }

        void OnInitializeCutIn()
        {
            StartCoroutine(Co_Delay());

            IEnumerator Co_Delay()
            {
                yield return new WaitForSeconds(0.1f);
                TurnPlace();
            }
        }

        void OnButton_TurnEnd()
        {
            if (++ProgressPlayerIndex < PlayerManager.Singleton.Players.Count)
            {
                Event_Turn_TurnEnd?.Invoke();

                TurnPlace();
            }
            else
            {
                TurnAIAction();
            }
        }
    }
}