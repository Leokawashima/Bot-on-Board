using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// タイトルシーンを管理するクラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField] TitlePressAnyKey m_pressAnyKey;
    [SerializeField] TitleMenu m_menu;
    [SerializeField] TitleCredit m_credit;
    [SerializeField] FadePanelSystem m_creditFadePanelSystem;

#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    bool m_isEntered = false;

    InputActionMapSettings m_input;

    void OnEnable()
    {
        m_input = new();
        m_input.UI.Any.started += OnClickAnyInput;
        m_input.Enable();
    }
    void OnDisable()
    {
        m_input.UI.Any.started -= OnClickAnyInput;
        m_input.Disable();
    }

    void OnClickAnyInput(InputAction.CallbackContext context_)
    {
        if (m_isEntered == false)
        {
            m_pressAnyKey.Enter();
        }
    }

    void Start()
    {
        m_pressAnyKey.Enable();
        m_menu.Disable();
        m_credit.Disable();
        m_creditFadePanelSystem.Disable();

        m_pressAnyKey.Initialize();
        m_menu.Initialize();
        m_credit.Initialize();

        m_pressAnyKey.FlashSystem.OnFlashFinished += () =>
        {
            m_menu.Enable();
            m_pressAnyKey.Disable();
        };

        m_menu.OnShowCredit += () =>
        {
            m_creditFadePanelSystem.Fade();
            m_creditFadePanelSystem.OnFadeInCompleted += m_credit.Enable;
            m_creditFadePanelSystem.OnFadeFinished += () =>
            {
                m_creditFadePanelSystem.OnFadeInCompleted -= m_credit.Enable;
            };
        };

        m_credit.OnHideCredit += () =>
        {
            m_creditFadePanelSystem.Fade();
            m_creditFadePanelSystem.OnFadeInCompleted += m_credit.Disable;
            m_creditFadePanelSystem.OnFadeFinished += () =>
            {
                m_creditFadePanelSystem.OnFadeInCompleted -= m_credit.Disable;
            };
        }; 
    }
}
