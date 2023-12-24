using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupDialog : MonoBehaviour
{
    [SerializeField] Canvas m_canvas;

    [field: SerializeField] public TMP_Text DialogText { get; private set; }

    [field: SerializeField] public Button AcceptButton { get; private set; }
    [field: SerializeField] public TMP_Text AcceptText { get; private set; }

    [field: SerializeField] public Button CancelButton { get; private set; }
    [field: SerializeField] public TMP_Text CancelText { get; private set; }

    public void Initialize(string text_)
    {
        DialogText.text = text_;
    }

    public void Initialize(string text_, string acceptText_, string cancelText_)
    {
        DialogText.text = text_;
        AcceptText.text = acceptText_;
        CancelText.text = cancelText_;
    }

    public void Enable()
    {
        m_canvas.enabled = true;
    }
    public void Disable()
    {
        m_canvas.enabled = false;
    }

#if UNITY_EDITOR
    [SerializeField] private string
        m_dialogText,
        m_acceptText,
        m_cancelText;

    private void OnValidate()
    {
        DialogText.text = m_dialogText;
        AcceptText.text = m_acceptText;
        CancelText.text = m_cancelText;
    }
#endif
}