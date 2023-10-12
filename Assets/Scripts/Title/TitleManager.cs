using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    public enum State { WaitAnyInput, WaitMenu, WaitCredits, }
    public State state { get; private set; } = State.WaitAnyInput;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject room;

    [SerializeField] T_PressAnyKeyScript m_PressAnyKey;
    [SerializeField] T_MenuScript m_Menu;
    [SerializeField] T_CreditsScript m_Credits;
    [SerializeField] FadePanelSystem m_FadePanelSystem;

    void OnEnable()
    {
        var map = new InputActionMapSettings();
        map.UI.Any.started += OnClick_AnyInput;
        map.Enable();
    }
    void OnDisable()
    {
        var map = new InputActionMapSettings();
        map.UI.Any.started -= OnClick_AnyInput;
        map.Disable();
    }

    void OnClick_AnyInput(InputAction.CallbackContext context)
    {
        if (state == State.WaitAnyInput)
        {
            SetState(state);
        }
    }

    void Start()
    {
        m_PressAnyKey.Enable();
        m_Menu.Disable();
        m_Credits.Disable();
        m_FadePanelSystem.Disable();

        m_PressAnyKey.Flash.OnFlashFinish += m_Menu.Enable;
        m_PressAnyKey.Flash.OnFlashFinish += m_PressAnyKey.Disable;

        m_Menu.OnMenuCredits += () =>
        {
            m_FadePanelSystem.Fade();
            m_FadePanelSystem.OnFadeComplete += m_Credits.Enable;
            m_FadePanelSystem.OnFadeEnd += () =>
            {
                m_FadePanelSystem.OnFadeComplete -= m_Credits.Enable;
            };
        };

        m_Credits.OnHideCredits += () =>
        {
            m_FadePanelSystem.Fade();
            m_FadePanelSystem.OnFadeComplete += m_Credits.Disable;
            m_FadePanelSystem.OnFadeEnd += () =>
            {
                m_FadePanelSystem.OnFadeComplete -= m_Credits.Disable;
            };
        }; 
    }

    void SetState(State state_)
    {
        switch(state_)
        {
            case State.WaitAnyInput:
                m_PressAnyKey.EnterEvent();
                state = State.WaitMenu;
                break;
            case State.WaitMenu:
                break;
        }
    }
}
