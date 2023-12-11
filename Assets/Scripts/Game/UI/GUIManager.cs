using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using ZXing.OneD;

/// <summary>
/// GUIを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class GUIManager : MonoBehaviour
{
    public static GUIManager Singleton { get; private set; }

    [SerializeField] TurnCountManager m_TurnCountManager;
    [SerializeField] AIHPUIManager m_AIHPUIManager;
    [SerializeField] PlayerUIManager m_PlayerUIManager;
    [SerializeField] CutInManager m_CutInManager;
    [SerializeField] DamageUIManager m_DamageUIManager;
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
        GameManager.Event_Initialize += OnInitialize;
        GameManager.Event_Turn_Place += OnTurnPlace;
        GameManager.Event_Turn_TurnEnd += OnTurnEnd;
        GameManager.Event_Turn_AIAction += OnAIAction;
        GameManager.Event_Turn_Initialize += OnTurnInitialize;
        GameManager.Event_Turn_GameSet += OnTurnGameSet;
    }
    void OnDisable()
    {
        GameManager.Event_Initialize -= OnInitialize;
        GameManager.Event_Turn_Place -= OnTurnPlace;
        GameManager.Event_Turn_TurnEnd -= OnTurnEnd;
        GameManager.Event_Turn_AIAction -= OnAIAction;
        GameManager.Event_Turn_Initialize -= OnTurnInitialize;
        GameManager.Event_Turn_GameSet -= OnTurnGameSet;
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
        m_TurnCountManager.SetTurn(GameManager.Singleton.ElapsedTurn);
        m_DamageUIManager.Initialize();

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
        m_TurnCountManager.SetTurn(GameManager.Singleton.ElapsedTurn);
        foreach(var ui_ in m_PlayerUIArray)
            ui_.TurnInitialize();

        m_CutInManager.CutIn("Turn:" + GameManager.Singleton.ElapsedTurn, () =>
        {
            Event_TurnInitializeCutIn?.Invoke();
        });
    }
    void OnTurnPlace()
    {
        m_CutInManager.CutIn("Place:" + GameManager.Singleton.PlayerIndex, () =>
        {
            m_PlayerUIArray[GameManager.Singleton.PlayerIndex].gameObject.SetActive(true);
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

    public void InitializeAIHPUI()
    {
        m_AIHPUIManager.Initialize(AIManager.Singleton.AIList);
    }
    public void DamageEffect(AISystem ai_, float power_)
    {
        m_DamageUIManager.AddUI(ai_, power_);
    }
}