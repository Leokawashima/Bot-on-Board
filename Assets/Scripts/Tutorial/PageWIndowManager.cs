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
        }
        m_canvas.enabled = false;
    }

    public void Enable(int index_)
    {
        m_canvas.enabled = true;
        m_target = m_windows[index_];
        SetText();
    }

    private void OnClose()
    {
        m_canvas.enabled = false;
        Event_Closed?.Invoke();
    }
    private void OnForward()
    {
        m_target.PageForward();
        SetText();
    }
    private void OnBackward()
    {
        m_target.PageBackward();
        SetText();
    }
    private void SetText()
    {
        m_numberText.text = $"{m_target.Index} / {m_target.Size}";
    }
}