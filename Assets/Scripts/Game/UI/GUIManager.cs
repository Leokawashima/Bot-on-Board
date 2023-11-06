using System;
using System.Collections;
using UnityEngine;
using ZXing.OneD;

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
    [SerializeField] AudioSource m_audio;
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    PlayerUIManager[] m_PlayerUIArray;

    public static event Action Event_TurnInitializeCutIn;
    public static event Action Event_ButtonPlace;
    public static event Action Event_ButtonTurnEnd;
    public static event Action Event_AICutInFinish;
    public static event Action Event_AnimGameSet;

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
        m_TurnCountManager.SetTurn(GameSystem.Singleton.ElapsedTurn);//下層は完成
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
        m_TurnCountManager.SetTurn(GameSystem.Singleton.ElapsedTurn);
        foreach(var ui_ in m_PlayerUIArray)
            ui_.TurnInitialize();

        m_CutInManager.CutIn("Turn:" + GameSystem.Singleton.ElapsedTurn, () =>
        {
            Event_TurnInitializeCutIn?.Invoke();
        });
    }
    void OnTurnPlace()
    {
        m_CutInManager.CutIn("Place:" + GameSystem.Singleton.PlayerIndex, () =>
        {
            m_PlayerUIArray[GameSystem.Singleton.PlayerIndex].gameObject.SetActive(true);
        });
    }
    void OnTurnEnd()
    {

    }
    void OnAIAction()
    {
        m_CutInManager.CutIn("AIAction", () =>
        {
            Event_AICutInFinish?.Invoke();
        });
    }
    void OnTurnGameSet()
    {
        m_audio.Play();
        m_CutInManager.CutIn("GameSet", () =>
        {
            Event_AnimGameSet?.Invoke();
        });
    }

    public void OnSetHPText(int index_, float hp_)
    {
        m_AIHPUIManager.Refresh(index_, hp_);
    }
}