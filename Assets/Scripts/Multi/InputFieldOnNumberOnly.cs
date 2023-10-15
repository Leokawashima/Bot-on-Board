using UnityEngine;
using TMPro;

/// <summary>
/// 正の整数のみ入力を許可するクラス
/// </summary>
/// 参考:https://nekojara.city/unity-input-field-content-type
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class InputFieldOnNumberOnly : MonoBehaviour
{
    [SerializeField] TMP_InputField m_inputField;
    [SerializeField] int m_valueMax = 100;

    void Start()
    {
        m_inputField.onValidateInput += OnValidateInputField;
    }

    /// <summary>
    /// InputFieldに文字入力があったときに呼ばれるコールバック
    /// </summary>
    char OnValidateInputField(string text_, int index_, char add_)
    {
        // 数値か文字かどうかを判定する 数値以外は入力を行わない
        if(char.IsDigit(add_) == false)
            return '\0';

        // 文字列が空じゃないかどうか判定する
        if(m_inputField.text != string.Empty)
        {
            // 文字列が既に最大値を上回っている時 最大値に固定
            if(int.Parse(m_inputField.text) > m_valueMax)
            {
                m_inputField.text = m_valueMax.ToString();
                return '\0';
            }
            // 文字列と入力した値の合計が最大値を上回っている時 最大値に固定
            if(int.Parse(m_inputField.text) * 10 + char.GetNumericValue(add_) > m_valueMax)
            {
                m_inputField.text = m_valueMax.ToString();
                return '\0';
            }
        }

        return add_;
    }
}