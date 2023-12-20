using System;
using System.Collections;
using Unity.Netcode;
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

        [field: SerializeField] public int TurnElapsed { get; private set; } = 1;
        [field: SerializeField] public int ProcessingPlayerIndex { get; private set; } = 0;
        [field: SerializeField] public int TurnSuddenDeath { get; private set; } = 30;
        [field: SerializeField] public int ForceFinishTurn { get; private set; } = 50;

        public static event Action
            Event_Initialize,
            Event_Turn_Initialize,
            Event_Turn_Place,
            Event_Turn_TurnEnd,
            Event_Turn_AIAction,
            Event_Turn_GameSet,
            Event_Turn_Finalize,
            Event_Finalize;

        public const int
            PLAYER_SIZE = 2,
            AI_SIZE = 2;

#if UNITY_EDITOR
        [SerializeField] private GameDebug m_debug;
#endif

        #region EventSubscribe
        void OnEnable()
        {
            //演出系の処理を作ったらそっちのコールバックイベント等につなぎを任せる
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

            GlobalSystem.SetGameMode(GameModeState.Local);
            switch (GlobalSystem.GameMode)
            {
                case GameModeState.Tutorial:
                    TutorialModeInitialize();
                    break;
                case GameModeState.Local:
                    LocalModeInitialize();
                    break;
                case GameModeState.Multi:
                    MultiModeInitialize();
                    break;
            }

            Event_Initialize?.Invoke();
        }
        void SystemFinalize()
        {
            Event_Finalize?.Invoke();

            Initiate.Fade(Name.Scene.Result, Color.black, 1.0f);
        }

        void TurnInitialize()
        {
            ProcessingPlayerIndex = 0;

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

            else if (TurnElapsed < ForceFinishTurn)
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

        void TutorialModeInitialize()
        {

        }
        void LocalModeInitialize()
        {
            Destroy(NetworkManager.Singleton.gameObject);
            Destroy(NetConnectManager.Singleton);
        }
        void MultiModeInitialize()
        {
            #region MultiItit
            /*
            //接続するまで待機をする間初期化をしておく
            //一定時間で戻りますか？のUIを出す

            //プレイヤー接続完了時のコールバック
            m_NetConnectManager.Event_PlayersConnected += () =>
            {
                m_RollButton.interactable = true;
                m_DiceSystem.gameObject.SetActive(true);
            };
            //プレイヤーが一人でも切断された時のコールバック
            m_NetConnectManager.Event_PlayersDisconnected += () =>
            {
                m_NetCautionUI.gameObject.SetActive(true);
            };

            m_RollButton.interactable = false;
            m_DiceSystem.gameObject.SetActive(false);

            m_HostButton.onClick.AddListener(() =>
            {
                m_NetConnectManager.Host();

                m_GameState = GameState.DecidedTheOrder;

                m_HostButton.interactable = false;
                m_ClientButton.interactable = false;
            });
            m_ClientButton.onClick.AddListener(() =>
            {
                m_NetConnectManager.Client();

                m_GameState = GameState.DecidedTheOrder;

                m_HostButton.interactable = false;
                m_ClientButton.interactable = false;
            });
            m_RollButton.onClick.AddListener(async () =>
            {
                m_RollButton.interactable = false;
                int _result = await m_DiceSystem.GetResult();
                _result = 2;
                m_Text.text = "Your Dice :" + _result;
                NetworkPlayerManager.m_Local.Roll.Value = _result;

                await Task.Delay(1000);
                m_DiceSystem.gameObject.SetActive(false);

                //ダイスを全員振るまで
                while(true)
                {
                    var _endFlag = true;
                    foreach(NetworkPlayerManager p in NetworkPlayerManager.m_List)
                    {
                        if(p.Roll.Value == 0) _endFlag = false;
                    }
                    if(_endFlag)
                    {
                        break;
                    }
                    await Task.Delay(100);
                }

                //ダイス目がかぶっていないかで分岐
                //人数が増えたら被ってない人と被った数値を比べて先に順番を決め、そのあと被りだけ振り直しのような処理にする
                if(NetworkPlayerManager.m_List[0].Roll.Value != NetworkPlayerManager.m_List[1].Roll.Value)
                {
                    //ダイス目のみのリストを作成、降順ソートを行う(昇順ソート＆反転)
                    var _orders = new List<int>();
                    foreach(NetworkPlayerManager p in NetworkPlayerManager.m_List)
                        _orders.Add(p.Roll.Value);
                    _orders.Sort();
                    _orders.Reverse();

                    //ダイス目とそのPlayerManagerのインスタンスを入れておくDictionaryを作成
                    //Keyにすることでこの後にMpalyerManager.m_Localで値をとれるのですっきり収まって個人的にすごい気に入っている
                    var _resultDictionary = new Dictionary<NetworkPlayerManager, int>();
                    foreach(NetworkPlayerManager p in NetworkPlayerManager.m_List)
                        _resultDictionary.Add(p, p.Roll.Value);

                    //自身のインスタンスだけデータにセット
                    // (Hostがデータ権限を持つとHostが過負荷になる＆設計的に書き込み権を各々のローカルにしか許容してない)
                    //Dictionaryからローカルインスタンスをキーに値を取り出し、降順ソートしたリストからその値でインデックスを確保
                    //...つまり大きい番号順に先攻にしている　わかりづらいよね
                    NetworkPlayerManager.m_Local.OrderIndex.Value = _orders.IndexOf(_resultDictionary[NetworkPlayerManager.m_Local]);

                    GameInitialize();
                }
                else
                {
                    NetworkPlayerManager.m_Local.ResetDice();
                    m_Text.text = "Reset";
                    m_DiceSystem.ResetDice();
                    m_RollButton.interactable = true;
                    m_DiceSystem.gameObject.SetActive(true);
                }
            });
            */
            #endregion
        }

        void OnMapCreated()
        {
            //本来ローカル専用初期化処理だが用意の時間が足りなかったので直書き
            var _playerManager = PlayerManager.Singleton.gameObject;
            _playerManager.AddComponent<PlayerInputManager>();
            _playerManager.AddComponent<LocalPlayerManager>();

            PlayerManager.Singleton.Initialize();
            BotManager.Singleton.Initialize();
            //ここまでローカル専用処理

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
            if (++ProcessingPlayerIndex < 2)//人数に応じたものにする
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