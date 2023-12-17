using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoAIOrder : MonoBehaviour
{
#if UNITY_EDITOR
    [field: SerializeField]
#endif
    public int Index { get; private set; } = -1;

    [SerializeField] Toggle m_toggle;
    [SerializeField] TMP_Text m_orderText;

    public void Initialize(string text_, int index_, ToggleGroup toggleGroup_)
    {
        Index = index_;
        m_toggle.group = toggleGroup_;
        m_orderText.text = text_;
    }
}