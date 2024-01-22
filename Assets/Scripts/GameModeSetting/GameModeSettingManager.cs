using UnityEngine;

public class GameModeSettingManager : MonoBehaviour
{
    [SerializeField] private InfoScrollViewManager m_infoScrollView;
    [SerializeField] private GameSettingManager m_gameSetting;
    [SerializeField] private PlayerSettingManager m_playerSetting;
    [SerializeField] private BotSettingManager m_botSetting;

    [SerializeField] private InfoPlayerSettingManager m_manager;
    [SerializeField] private ShowInfoPlayerSetting m_show;
    [SerializeField] private EditInfoBotSetting edit;

    private void Start()
    {
        Initiaiize();
    }
    public void Initiaiize()
    {
        m_infoScrollView.Initialize();
        m_gameSetting.Initialize();
        m_playerSetting.Initialize();
        m_botSetting.Initialize();

        m_manager.Initialize();
        m_show.Initialize();
        edit.Initialize();
    }
}
