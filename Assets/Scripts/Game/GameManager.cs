using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Map;

/// <summary>
/// ゲームの基本的な処理を担うクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] DiceSystem m_DiceSystem;
    [SerializeField] NetConnectManager m_NetConnectManager;
    [SerializeField] NetworkManager m_NetworkManager;
    [SerializeField] AIManager m_AIManager;
    [SerializeField] CutInSystem m_cutInSystem;

    public static GameManager Singleton { get; private set; }

    public enum GameState { Non, Initialize, DecidedTheOrder, Battle, GameSet, Finalize }
    public GameState m_GameState = GameState.Non;
    public enum BattleState { Non, Initialize, Place, AIAction, Finalize, GameSet }
    public BattleState m_BattleState = BattleState.Non;

    [field: SerializeField] public int ElapsedTurn { get; private set; } = 1;
    [field: SerializeField] public int PlayerIndex { get; private set; } = 0;
    //[field: SerializeField] public int m_SuddenDeathTurn { get; private set; } = 30;
    [field: SerializeField] public int ForceFinishTurn { get; private set; } = 50;

    public static event Action Event_Initialize;
    public static event Action Event_Turn_Initialize;
    public static event Action Event_Turn_Place;
    public static event Action Event_Turn_TurnEnd;
    public static event Action Event_Turn_AIAction;
    public static event Action Event_Turn_GameSet;
    public static event Action Event_Turn_Finalize;
    public static event Action Event_Finalize;

#region EventSubscribe
    void OnEnable()
    {
        //演出系の処理を作ったらそっちのコールバックイベント等につなぎを任せる
        MapManager.Event_MapCreated += OnMapCreated;
        GUIManager.Event_TurnInitializeCutIn += OnInitializeCutIn;
        PlayerUIManager.Event_ButtonTurnEnd += OnButton_TurnEnd;
        GUIManager.Event_AnimGameSet += SystemFinalize;
        AIManager.Event_AiActioned += TurnFinalize;
    }
    void OnDisable()
    {
        MapManager.Event_MapCreated -= OnMapCreated;
        GUIManager.Event_TurnInitializeCutIn -= OnInitializeCutIn;
        PlayerUIManager.Event_ButtonTurnEnd -= OnButton_TurnEnd;
        GUIManager.Event_AnimGameSet -= SystemFinalize;
        AIManager.Event_AiActioned -= TurnFinalize;
    }
#endregion EventSubscribe

#region Singleton
    void Awake()
    {
        Singleton ??= this;
    }
    void OnDestroy()
    {
        Singleton = null;
    }
#endregion Singleton

    void Start()
    {
        SystemInitalize();
    }

    void SystemInitalize()
    {
        m_BattleState = BattleState.Non;
        UnityEngine.Random.InitState(DateTime.Now.Millisecond + DateTime.Now.Second);
        CameraManager.Singleton.SetFreeLookCamIsMove(false);

        GlobalSystem.SetMatchState(GlobalSystem.MatchState.Local);
        switch(GlobalSystem.m_MatchState)
        {
            case GlobalSystem.MatchState.Tutorial:
                TutorialModeInitialize();
                break;
            case GlobalSystem.MatchState.Local:
                LocalModeInitialize();
                break;
            case GlobalSystem.MatchState.Multi:
                MultiModeInitialize();
                break;
        }

        m_GameState = GameState.Initialize;
        Event_Initialize?.Invoke();
    }
    void SystemFinalize()
    {
        m_BattleState = BattleState.Non;

        m_GameState = GameState.Finalize;
        Event_Finalize?.Invoke();

        Initiate.Fade(Name.Scene.Result, Color.black, 1.0f);
    }

    void TurnInitialize()
    {
        PlayerIndex = 0;

        m_BattleState = BattleState.Initialize;
        Event_Turn_Initialize?.Invoke();
    }
    void TurnPlace()
    {
        m_BattleState = BattleState.Place;
        Event_Turn_Place?.Invoke();
    }
    void TurnAIAction()
    {
        m_BattleState = BattleState.AIAction;
        Event_Turn_AIAction?.Invoke();
    }
    void TurnFinalize()
    {
        m_BattleState = BattleState.Finalize;
        Event_Turn_Finalize?.Invoke();
        
        if (m_AIManager.CheckAIIsDead())
            TurnGameSet();

        else if(ElapsedTurn < ForceFinishTurn)
        {
            ElapsedTurn++;

            TurnInitialize();
        }
        else
        {
            TurnGameSet();
        }
    }
    void TurnGameSet()
    {
        m_GameState = GameState.GameSet;
        Event_Turn_GameSet?.Invoke();
    }

    void TutorialModeInitialize()
    {
        
    }
    void LocalModeInitialize()
    {
        Destroy(m_NetworkManager.gameObject);
    }
    void MultiModeInitialize()
    {
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
    }

    void OnMapCreated()
    {
        //本来ローカル専用初期化処理だが用意の時間が足りなかったので直書き
        var player = new GameObject("PlayerManager");
        player.AddComponent<PlayerInputManager>();
        player.AddComponent<LocalPlayerManager>();

        m_AIManager.Initialize();
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
        if (++PlayerIndex < 2)//人数に応じたものにする
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