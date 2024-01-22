using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PopupDialog : SingletonMonoBehaviour<PopupDialog>
{
    [SerializeField] private Canvas m_canvas;

    [SerializeField] private TMP_Text m_dialogText;
    public static TMP_Text DialogText => Singleton.m_dialogText;
    [SerializeField] private TMP_Text m_acceptText;
    public static TMP_Text AcceptText => Singleton.m_acceptText;
    [SerializeField] private TMP_Text m_cancelText;
    public static TMP_Text CancelText => Singleton.m_cancelText;

    [SerializeField] private Button m_acceptButton;
    [SerializeField] private Button m_cancelButton;

    public static void Enable() => Singleton.m_canvas.enabled = true;
    public static void Disable() => Singleton.m_canvas.enabled = false;

    public static void Enable(string dialog_, UnityAction acceptCallback_)
    {
        DialogText.text = dialog_;
        Singleton.m_acceptButton.onClick.AddListener(acceptCallback_);
        Enable();
    }
    public static void Enable(string dialog_, UnityAction acceptCallback_, UnityAction cancelCallback_)
    {
        DialogText.text = dialog_;
        Singleton.m_acceptButton.onClick.AddListener(acceptCallback_);
        Singleton.m_cancelButton.onClick.AddListener(cancelCallback_);
        Enable();
    }

    public static void SetText(string dialog_)
    {
        DialogText.text = dialog_;
    }
    public static void SetText(string dialog_, string accept_, string cancel_)
    {
        DialogText.text = dialog_;
        AcceptText.text = accept_;
        CancelText.text = cancel_;
    }

    public void RemoveCallback()
    {
        m_acceptButton.onClick.RemoveAllListeners();
        m_cancelButton.onClick.RemoveAllListeners();
    }

#if UNITY_EDITOR
    [SerializeField] private string m_dialogPreview;
    [SerializeField] private string m_acceptPreview;
    [SerializeField] private string m_cancelPreview;

    private void OnValidate()
    {
        m_dialogText.text = m_dialogPreview;
        m_acceptText.text = m_acceptPreview;
        m_cancelText.text = m_cancelPreview;
    }
#endif
}