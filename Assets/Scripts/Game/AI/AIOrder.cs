using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIOrder : MonoBehaviour
{
    [SerializeField] Toggle m_Toggle;
    [SerializeField] TextMeshProUGUI m_OrderText;
    public int Index { get; private set; } = 0;

    public void Initialize(string text_, int index_, ToggleGroup toggleGroup_)
    {
        m_OrderText.text = text_;
        Index = index_;
        m_Toggle.group = toggleGroup_;
    }
}
