using System;
using UnityEngine;

/// <summary>
/// GUI全般の管理クラス
/// </summary>
public class GUIManager : MonoBehaviour
{
    public static GUIManager Singleton { get; private set; }

    [SerializeField] TurnCountManager m_TurnCountManager;
    [SerializeField] InfoPlayerDataManager m_infoPlayerDataManager;
    [SerializeField] PlayerUIManager m_PlayerUIManager;
    [SerializeField] CutInManager m_CutInManager;
    [SerializeField] DamageUIManager m_DamageUIManager;
    [SerializeField] AudioSource m_audio;

    public static event Action Event_TurnInitializeCutIn;
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

        m_PlayerUIManager.Initialize();
    }
    void OnTurnInitialize()
    {
        m_TurnCountManager.SetTurn(GameManager.Singleton.ElapsedTurn);
        m_PlayerUIManager.TurnInitialize();

        m_CutInManager.CutIn("Turn:" + GameManager.Singleton.ElapsedTurn, () =>
        {
            Event_TurnInitializeCutIn?.Invoke();
        });
    }
    void OnTurnPlace()
    {
        m_CutInManager.CutIn("Place:" + GameManager.Singleton.PlayerIndex, () =>
        {
            m_PlayerUIManager.TurnPlace();
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
        m_infoPlayerDataManager.Refresh(index_, hp_);
    }

    public void InitializeAIHPUI()
    {
        m_infoPlayerDataManager.Initialize(AIManager.Singleton.AIList);
    }
    public void DamageEffect(AISystem ai_, float power_)
    {
        m_DamageUIManager.AddUI(ai_, power_);
    }
}