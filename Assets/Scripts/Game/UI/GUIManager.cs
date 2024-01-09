using UnityEngine;
using Bot;

/// <summary>
/// GUI全般の管理クラス
/// </summary>
public class GUIManager : SingletonMonoBehaviour<GUIManager>
{
    [SerializeField] TurnCountManager m_turnCountManager;
    [SerializeField] InfoBotStatusManager m_infoPlayerDataManager;
    [field: SerializeField] public PlayerUIManager PlayerUI { get; private set; }
    [field: SerializeField] public CutInSystem CutIn { get; private set; }

    [SerializeField] FloatingUIManager m_floatingUI;

    public void Initialize()
    {
        m_floatingUI.Initialize();

        PlayerUI.Initialize();
        CutIn.Disable();
    }

    // 右側に出るプレイヤーのデータ
    public void InitializeInfoPlayerData()
    {
        m_infoPlayerDataManager.Initialize();
    }
    public void Refresh(BotAgent bot_)
    {
        m_infoPlayerDataManager.Refresh(bot_.Operator.Index, bot_);
    }

    // FloatUI
    public void DamageEffect(BotAgent bot_, float power_)
    {
        m_floatingUI.AddUI(bot_, power_, Color.red);
    }
    public void HealEffect(BotAgent bot_, float power_)
    {
        m_floatingUI.AddUI(bot_, power_, Color.green);
    }
    public void InteliEffect(BotAgent bot_, int difference_)
    {
        m_floatingUI.AddUI(bot_, difference_, Color.blue);
    }
}