using System.Collections.Generic;
using UnityEngine;

public class EditInfoBotSetting : MonoBehaviour
{
    [SerializeField] private PlusMinusButton m_hpPMButton;
    [SerializeField] private PlusMinusButton m_hpMaxPMButton;
    [SerializeField] private PlusMinusButton m_attackPMButton;

    [SerializeField] private InfoBotSettingManager m_prefab;
    [SerializeField] private Transform m_content;

    [SerializeField] private PlusMinusButton m_playerPMButton;
    [SerializeField] private PlusMinusButton m_botOperationsPMButton;
    [SerializeField] private InfoPlayerSettingManager m_showInfoPlayerSetting;

#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<InfoBotSettingManager> m_managers;

    public void Initialize()
    {
        var _players = m_showInfoPlayerSetting.Infos.Count;
        m_managers = new(m_playerPMButton.ValueMax);
        for (int i = 0; i < _players; ++i)
        {
            var _manager = Instantiate(m_prefab, m_content);
            _manager.Initialize(m_botOperationsPMButton.ValueMax, m_showInfoPlayerSetting.Infos[i].Data.BotSettings);
            m_managers.Add(_manager);
        }
    }
}