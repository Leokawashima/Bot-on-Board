using UnityEngine;
using UnityEngine.UI;

namespace GameMode
{
    public class GameModeManager : MonoBehaviour
    {
        private void Start()
        {
            Initiaiize();
        }

        [SerializeField] private ModeChoiceManager m_modeChoice;
        [SerializeField] private InfoScrollViewManager m_infoScrollView;
        [SerializeField] private GameSettingManager m_gameSetting;
        [SerializeField] private PlayerSettingManager m_playerSetting;
        [SerializeField] private BotSettingManager m_botSetting;

        private void Initiaiize()
        {
            m_modeChoice.Initialize();
            m_infoScrollView.Initialize();
            m_gameSetting.Initialize();
            m_playerSetting.Initialize();
            m_botSetting.Initialize();

            m_gameSetting.Enable();
            m_playerSetting.Disable();
            m_botSetting.Disable();
        }
    }
}