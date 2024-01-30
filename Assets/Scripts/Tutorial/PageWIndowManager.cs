using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PageWindowManager : MonoBehaviour
{
    [SerializeField] private Canvas m_canvas;

    [SerializeField] private Button m_closeButton;
    [SerializeField] private Button m_forwardButton;
    [SerializeField] private Button m_backwardButton;
    [SerializeField] private TMP_Text m_numberText;

    [SerializeField] private PageWindow[] m_windows;

    private PageWindow m_target;

    public event Action Event_Closed;

    public void Initialize()
    {
        m_closeButton.onClick.AddListener(OnClose);
        m_forwardButton.onClick.AddListener(OnForward);
        m_backwardButton.onClick.AddListener(OnBackward);

        foreach (var window in m_windows)
        {
            window.Initialize();
            window.gameObject.SetActive(false);
        }
        Enable(false);
    }

    public void Enable(int index_)
    {
        Enable(true);
        m_target = m_windows[index_];
        m_target.gameObject.SetActive(true);
        SetIntaractable();
        SetText();
    }

    private void Enable(bool active_)
    {
        m_canvas.enabled = active_;
        gameObject.SetActive(active_);
    }

    private void OnClose()
    {
        Enable(false);
        m_target.gameObject.SetActive(false);
        Event_Closed?.Invoke();
    }
    private void OnForward()
    {
        m_target.PageForward();
        SetIntaractable();
        SetText();
    }
    private void OnBackward()
    {
        m_target.PageBackward();
        SetIntaractable();
        SetText();
    }
    private void SetText()
    {
        m_numberText.text = $"{m_target.Index + 1} / {m_target.Size}";
    }

    private void SetIntaractable()
    {
        m_forwardButton.interactable = m_target.Index != m_target.Size - 1;
        m_backwardButton.interactable = m_target.Index != 0;
    }
}