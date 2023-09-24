using UnityEngine;
using TMPro;

/// <summary>
/// 正の整数のみ入力を許可するクラス
/// </summary>
/// 参考:https://nekojara.city/unity-input-field-content-type
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class InputFieldOnNumberOnly : MonoBehaviour
{
    [SerializeField] TMP_InputField m_InputField;
    [SerializeField] int m_Max = 100;

    void Awake()
    {
        m_InputField.onValidateInput += OnValidateInputField;
    }

    char OnValidateInputField(string text_, int index_, char add_)
    {
        if(!char.IsDigit(add_)) return '\0';
        if(m_InputField.text != string.Empty)
        {
            if(int.Parse(m_InputField.text) > m_Max) m_InputField.text = m_Max.ToString();
            if(int.Parse(m_InputField.text) * 10 + char.GetNumericValue(add_) > m_Max) m_InputField.text = m_Max.ToString();
        }
        return add_;
    }
}
