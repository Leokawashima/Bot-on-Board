﻿using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// ゲームの基本的な処理を担うクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class GameSystem : MonoBehaviour
{
    [SerializeField] DiceSystem m_DiceSystem;
    [SerializeField] NetConnectManager m_NetConnectManager;
    [SerializeField] NetworkManager m_NetworkManager;
    [SerializeField] AIManager m_AIManager;

    public static GameSystem Singleton { get; private set; }

    public enum GameState { Non, Initialize, DecidedTheOrder, Battle, GameSet, Finalize }
    public GameState m_GameState = GameState.Non;
    public enum BattleState { Non, Initialize, Place, AIAction, Finalize, GameSet }
    public BattleState m_BattleState = BattleState.Non;

    //デバッグ用にプロパティを外しているだけ
    public int m_ElapsedTurn = 1;
    public int m_PlayerIndex = 0;
    //public int m_SuddenDeathTurn  = 30;
    public int m_ForceFinishTurn = 50;

    [SerializeField] List<AIManager> m_AIList = new();

    public static event Action Event_Initialize;
    public static event Action Event_Turn_Initialize;
    public static event Action Event_Turn_Place;
    public static event Action Event_Turn_TurnEnd;
    public static event Action Event_Turn_AIAction;
    public static event Action Event_Turn_GameSet;
    public static event Action Event_Turn_Finalize;
    public static event Action Event_Finalize;

    void OnEnable()
    {
        //演出系の処理を作ったらそっちのコールバックイベント等につなぎを任せる
        MapManager.Event_MapCreated += OnMapCreated;
        GUIManager.Event_ButtonTurnEnd += OnButton_TurnEnd;
    }
    void OnDisable()
    {
        MapManager.Event_MapCreated -= OnMapCreated;
        GUIManager.Event_ButtonTurnEnd -= OnButton_TurnEnd;
    }

    void Start()
    {
        Singleton ??= this;
        SystemInitalize();
    }
    void OnDestroy()
    {
        Singleton = null;
    }

    void SystemInitalize()
    {
        m_BattleState = BattleState.Non;
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
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
        m_PlayerIndex = 0;

        m_BattleState = BattleState.Initialize;
        Event_Turn_Initialize?.Invoke();

        TurnPlace();
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

        foreach(var _ai in m_AIList)//全員現在の状態から意思決定
            _ai.Think();

        foreach(var _ai in m_AIList)//前意思決定後に行動
            _ai.Move();

        TurnFinalize();
    }
    void TurnFinalize()
    {
        m_BattleState = BattleState.Finalize;
        Event_Turn_Finalize?.Invoke();

        if(m_ElapsedTurn < m_ForceFinishTurn)
        {
            m_ElapsedTurn++;

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

        SystemFinalize();
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
        var player = new GameObject("PlayerManager");
        player.AddComponent<PlayerInputManager>();
        player.AddComponent<LocalPlayerManager>();

        for(int i = 0; i < 2; ++i)//人数分処理する　現在は2固定
        {
            var _ai = Instantiate(m_AIManager, player.transform);
            _ai.Spawn($"AI:{i}", new Vector2Int(i * 9, i * 9));
            m_AIList.Add(_ai);
        }

        TurnInitialize();
    }

    void OnButton_TurnEnd()
    {
        if (++m_PlayerIndex < 2)//人数に応じたものにする
        {
            Event_Turn_TurnEnd?.Invoke();
        }
        else
        {
            TurnAIAction();
        }
    }
}