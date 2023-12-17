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
    [SerializeField] TitleCredit m_tutorial;
    [SerializeField] FadePanelSystem m_fadePanelSystem;

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
            m_isEntered = true;
            m_pressAnyKey.Enter();
        }
    }

    void Start()
    {
        m_pressAnyKey.Enable();
        m_menu.Disable();
        m_credit.Disable();
        m_tutorial.Disable();
        m_fadePanelSystem.Disable();

        m_pressAnyKey.Initialize();
        m_menu.Initialize();
        m_credit.Initialize();
        m_tutorial.Initialize();

        m_pressAnyKey.FlashSystem.Event_Finished += () =>
        {
            m_menu.Enable();
            m_pressAnyKey.Disable();
        };

        m_menu.OnShowCredit += () =>
        {
            m_fadePanelSystem.Fade();
            m_fadePanelSystem.OnFadeInCompleted += m_credit.Enable;
            m_fadePanelSystem.OnFadeFinished += () =>
            {
                m_fadePanelSystem.OnFadeInCompleted -= m_credit.Enable;
            };
        };

        m_menu.OnShowTutorial += () =>
        {
            m_fadePanelSystem.Fade();
            m_fadePanelSystem.OnFadeInCompleted += m_tutorial.Enable;
            m_fadePanelSystem.OnFadeFinished += () =>
            {
                m_fadePanelSystem.OnFadeInCompleted -= m_tutorial.Enable;
            };
        };

        m_credit.OnHideCredit += () =>
        {
            m_fadePanelSystem.Fade();
            m_fadePanelSystem.OnFadeInCompleted += m_credit.Disable;
            m_fadePanelSystem.OnFadeFinished += () =>
            {
                m_fadePanelSystem.OnFadeInCompleted -= m_credit.Disable;
            };
        };

        m_tutorial.OnHideCredit += () =>
        {
            m_fadePanelSystem.Fade();
            m_fadePanelSystem.OnFadeInCompleted += m_tutorial.Disable;
            m_fadePanelSystem.OnFadeFinished += () =>
            {
                m_fadePanelSystem.OnFadeInCompleted -= m_tutorial.Disable;
            };
        };
    }
}
