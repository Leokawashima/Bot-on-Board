using UnityEngine;
using TMPro;

public class ValueSetting : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private PlusMinusButton m_plusMinusButton;

    [SerializeField, TextArea] private string m_nameText;
    [SerializeField] private int m_value;
    [SerializeField] private int m_valueMin;
    [SerializeField] private int m_valueMax;

#if UNITY_EDITOR
    private void OnValidate()
    {
        m_text.text = m_nameText;
        m_plusMinusButton.SetValue(m_value);
        m_plusMinusButton.SetMin(m_valueMin);
        m_plusMinusButton.SetMax(m_valueMax);
    }
#endif
}