using System.Collections.Generic;
using UnityEngine;

public class BotSettingManager : SingletonMonoBehaviour<BotSettingManager>
{
    [SerializeField] private Canvas m_canvas;

    [SerializeField] private PlusMinusButton m_hpPMButton;
    [SerializeField] private PlusMinusButton m_hpMaxPMButton;
    [SerializeField] private PlusMinusButton m_attackPMButton;

    [SerializeField] private Transform m_content;
    [SerializeField] private InfoBotSettingManager m_prefab;

#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<InfoBotSettingManager> m_managers;

    public void Enable() => m_canvas.enabled = true;
    public void Disable() => m_canvas.enabled = false;

    public void Initialize()
    {
        var _playerInfos = InfoScrollViewManager.Singleton.Infos;
        var _playerPMButton = GameSettingManager.Singleton.PlayerPMButton;
        var _botPMButton = PlayerSettingManager.Singleton.BotPMButton;
        m_managers = new(_playerPMButton.ValueMax);
        for (int i = 0; i < _playerInfos.Count; ++i)
        {
            var _manager = Instantiate(m_prefab, m_content);
            _manager.Initialize(_botPMButton.ValueMax, _playerInfos[i].Data.BotSettings);
            m_managers.Add(_manager);
        }
    }
}