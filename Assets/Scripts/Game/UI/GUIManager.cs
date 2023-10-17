using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// GUIを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class GUIManager : MonoBehaviour
{
    public static GUIManager Singleton { get; private set; }

    [SerializeField] TurnCountManager m_TurnCountManager;//完成
    [SerializeField] AIHPUIManager m_AIHPUIManager;//完成
    [SerializeField] PlayerUIManager m_PlayerUIManager;
    [SerializeField] CutInManager m_CutInManager;//完成

#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    PlayerUIManager[] m_PlayerUIArray;

    public static event Action Event_TurnInitializeCutIn;
    public static event Action Event_ButtonPlace;
    public static event Action Event_ButtonTurnEnd;

    void OnEnable()
    {
        GameSystem.Event_Initialize += OnInitialize;
        GameSystem.Event_Turn_Place += OnTurnPlace;
        GameSystem.Event_Turn_TurnEnd += OnTurnEnd;
        GameSystem.Event_Turn_AIAction += OnAIAction;
        GameSystem.Event_Turn_Initialize += OnTurnInitialize;
        GameSystem.Event_Turn_GameSet += OnTurnGameSet;
    }
    void OnDisable()
    {
        GameSystem.Event_Initialize -= OnInitialize;
        GameSystem.Event_Turn_Place -= OnTurnPlace;
        GameSystem.Event_Turn_TurnEnd -= OnTurnEnd;
        GameSystem.Event_Turn_AIAction -= OnAIAction;
        GameSystem.Event_Turn_Initialize -= OnTurnInitialize;
        GameSystem.Event_Turn_GameSet -= OnTurnGameSet;
    }

    void Awake()
    {
        Singleton ??= this;
    }
    void OnDestroy()
    {
        Singleton = null;
    }

    void OnInitialize()
    {
        m_TurnCountManager.SetTurn(GameSystem.Singleton.m_ElapsedTurn);//下層は完成
        m_AIHPUIManager.Initialize(2, 10);//2人　HP 10で初期化 下層は完成

        //ローカルの場合は人数分
        m_PlayerUIArray = new PlayerUIManager[2];
        for(int i = 0; i < 2; ++i)
        {
            m_PlayerUIArray[i] = Instantiate(m_PlayerUIManager, transform);
            m_PlayerUIArray[i].Initialize();
            m_PlayerUIArray[i].Event_ButtonPlace += () =>
            {
                Event_ButtonPlace?.Invoke();
            };
            m_PlayerUIArray[i].Event_ButtonTurnEnd += () =>
            {
                Event_ButtonTurnEnd?.Invoke();
            };
            m_PlayerUIArray[i].gameObject.SetActive(false);
        }

        //マルチなら一つ分生成
    }
    void OnTurnInitialize()
    {
        m_TurnCountManager.SetTurn(GameSystem.Singleton.m_ElapsedTurn);
        foreach(var ui_ in m_PlayerUIArray)
            ui_.TurnInitialize();

        m_CutInManager.CutIn("Turn:" + GameSystem.Singleton.m_ElapsedTurn, () =>
        {
            Debug.Log("TurnCutinEnd");
            Event_TurnInitializeCutIn?.Invoke();
        });
    }
    void OnTurnPlace()
    {
        m_CutInManager.CutIn("Place:" + GameSystem.Singleton.m_PlayerIndex, () =>
        {
            Debug.Log("PlaceCutinEnd");
            m_PlayerUIArray[GameSystem.Singleton.m_PlayerIndex].gameObject.SetActive(true);
        });
    }
    void OnTurnEnd()
    {

    }
    void OnAIAction()
    {
        m_CutInManager.CutIn("AIAction");
    }
    void OnTurnGameSet()
    {
        m_CutInManager.CutIn("GameSet");
    }

    public void OnSetHPText(int index_, float hp_)
    {
        m_AIHPUIManager.Refresh(index_, hp_);
    }
}