using UnityEngine;

public class GameRuleSetting : MonoBehaviour
{
    [field: SerializeField] public int TurnSuddenDeath { get; private set; } = 30;
    [field: SerializeField] public int TurnForceFinish { get; private set; } = 50;

    [SerializeField] private InfoPlayerSettingManager m_manager;
    [SerializeField] EditInfoBotSetting edit;

    private void Start()
    {
        Initiaiize();
    }
    public void Initiaiize()
    {
        m_manager.Initialize();
        edit.Initialize();
    }
}