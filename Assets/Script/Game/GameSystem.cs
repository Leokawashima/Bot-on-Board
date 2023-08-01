using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class GameSystem : MonoBehaviour
{
    [SerializeField] SelectPlayManager selectPlayManager;
    [SerializeField] MapManager mapManager;
    [SerializeField] NetConnectManager netConnectManager;
    
    public static event Action GameIntializeEvent;
    public static event Action GameLoopEvent_First;
    public static event Action GameLoopEvent_Second;
    public static event Action GameLoopEvent_AIMove;
    public static event Action GameLoopEvent_TurnEnd;
    public static event Action GameFinalizeEvent;
    [SerializeField] int elapsedTurn = 1;

    CancellationTokenSource cts = new();

    void Start()
    {
        //とりあえずホストクライアントを自動で開始するようにするだけの処理
        if (RoomUDP.State == RoomUDP.RoomState.Host)
        {
            netConnectManager.Host();
        }
        else if (RoomUDP.State == RoomUDP.RoomState.Client)
        {
            netConnectManager.Client();
        }
        else
        {
            netConnectManager.Host();
        }

        selectPlayManager.Initialize();
        selectPlayManager.SetFinishEvent += GameInitialize;

        mapManager.OnMapCreated += () =>
        {
            GameIntializeEvent?.Invoke();
            GameFirst();
        };
    }

    void GameInitialize()
    {
        selectPlayManager.gameObject.SetActive(false);
        mapManager.MapCreate();
    }

    void GameFirst()
    {
        GameLoopEvent_First?.Invoke();
        GameSecond();
    }
    void GameSecond()
    {
        GameLoopEvent_Second?.Invoke();
        GameAIMove();
    }
    void GameAIMove()
    {
        GameLoopEvent_AIMove?.Invoke();

        if (elapsedTurn <= 3)
        {
            elapsedTurn++;
            GameLoopEvent_TurnEnd?.Invoke();
            GameFirst();
        }
        else
        {
            GameFinalize();
        }
    }

    void GameFinalize()
    {
        GameFinalizeEvent?.Invoke();
    }
}
