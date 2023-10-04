using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class GameSystem : MonoBehaviour
{
    [SerializeField] DiceSystem m_Dice;
    [SerializeField] MapManager mapManager;
    [SerializeField] NetConnectManager netConnectManager;

    public enum GameState { WaitConnect, DecidedTheOrder, Place, AIAction, GameSet }
    public GameState m_State = GameState.WaitConnect;

    [SerializeField] int m_ElapsedTurn = 1;

    [SerializeField] TextMeshProUGUI m_Text;
    [SerializeField] Button m_HostButton;
    [SerializeField] Button m_ClientButton;
    [SerializeField] Button m_RollButton;

    public static event Action GameIntializeEvent;
    public static event Action GameLoopEvent_First;
    public static event Action GameLoopEvent_Second;
    public static event Action GameLoopEvent_AIMove;
    public static event Action GameLoopEvent_TurnEnd;
    public static event Action GameFinalizeEvent;

    void Start()
    {
        mapManager.OnMapCreated += () =>
        {
            GameIntializeEvent?.Invoke();

            m_State = GameState.Place;

            GameFirst();
        };

        m_RollButton.interactable = false;
        m_Dice.gameObject.SetActive(false);

        m_HostButton.onClick.AddListener(() =>
        {
            netConnectManager.Host();

            m_State = GameState.DecidedTheOrder;

            m_HostButton.interactable = false;
            m_ClientButton.interactable = false;
            m_RollButton.interactable = true;
            m_Dice.gameObject.SetActive(true);
        });
        m_ClientButton.onClick.AddListener(() =>
        {
            netConnectManager.Client();

            m_State = GameState.DecidedTheOrder;

            m_HostButton.interactable = false;
            m_ClientButton.interactable = false;
            m_RollButton.interactable = true;
            m_Dice.gameObject.SetActive(true);
        });
        m_RollButton.onClick.AddListener(async () =>
        {
            m_RollButton.interactable = false;
            int _result = await m_Dice.GetResult();
            m_Text.text = "Your Dice :" + _result;
            PlayerManager.m_Local.roll.Value = _result;

            await Task.Delay(1000);
            m_Dice.gameObject.SetActive(false);

            while(true)
            {
                var _flag = false;
                foreach(PlayerManager p in PlayerManager.m_List)
                {
                    _flag = p.roll.Value != 0;
                }
                if(_flag)
                {
                    break;
                }
                await Task.Delay(100);
            }

            GameInitialize();
        });
    }

    void GameInitialize()
    {
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

        if (m_ElapsedTurn <= 3)
        {
            m_ElapsedTurn++;
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
