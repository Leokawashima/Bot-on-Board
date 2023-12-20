using System;
using UnityEngine;
using Bot;
using Game;

/// <summary>
/// GUI全般の管理クラス
/// </summary>
public class GUIManager : SingletonMonoBehaviour<GUIManager>
{
    [SerializeField] TurnCountManager m_turnCountManager;
    [SerializeField] InfoPlayerDataManager m_infoPlayerDataManager;
    [SerializeField] PlayerUIManager m_playerUIManager;
    [SerializeField] FloatingUIManager m_floatingUIManager;
    [SerializeField] CutInSystem m_cutInSystem;
    [SerializeField] AudioSource m_audio;

    public static event Action
        Event_TurnInitializeCutIn,
        Event_BotCutInFinish,
        Event_AnimGameSet;

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

    void OnInitialize()
    {
        m_turnCountManager.SetTurn(GameManager.Singleton.TurnElapsed);
        m_floatingUIManager.Initialize();

        m_playerUIManager.Initialize();
    }
    void OnTurnInitialize()
    {
        m_turnCountManager.SetTurn(GameManager.Singleton.TurnElapsed);
        m_playerUIManager.TurnInitialize();

        m_cutInSystem.CutIn($"Turn:{ GameManager.Singleton.TurnElapsed}", () =>
        {
            Event_TurnInitializeCutIn?.Invoke();
        });
    }
    void OnTurnPlace()
    {
        m_cutInSystem.CutIn($"Place:{GameManager.Singleton.ProgressPlayerIndex}", () =>
        {
            m_playerUIManager.TurnPlace();
        });
    }
    void OnTurnEnd()
    {

    }
    void OnAIAction()
    {
        m_cutInSystem.CutIn("AIAction", () =>
        {
            Event_BotCutInFinish?.Invoke();
        });
    }
    void OnTurnGameSet()
    {
        m_audio.Play();
        m_cutInSystem.CutIn("GameSet", () =>
        {
            Event_AnimGameSet?.Invoke();
        });
    }

    public void InitializeInfoPlayerData()
    {
        m_infoPlayerDataManager.Initialize();
    }
    public void Refresh(BotAgent bot_)
    {
        m_infoPlayerDataManager.Refresh(bot_.Operator.Index, bot_);
    }

    public void DamageEffect(BotAgent bot_, float power_)
    {
        m_floatingUIManager.AddUI(bot_, power_, Color.red);
    }
    public void HealEffect(BotAgent bot_, float power_)
    {
        m_floatingUIManager.AddUI(bot_, power_, Color.green);
    }
    public void InteliEffect(BotAgent bot_, int difference_)
    {
        m_floatingUIManager.AddUI(bot_, difference_, Color.blue);
    }
}