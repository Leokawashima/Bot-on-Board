using UnityEngine;
using TMPro;

/// <summary>
/// 正の整数のみ入力を許可し最大値を設定できる
/// </summary>
/// 参考:https://nekojara.city/unity-input-field-content-type
[CreateAssetMenu(fileName = "NumberValidator", menuName = "TextMeshPro/Number Validator")]
public class TMP_NumberValidator : TMP_InputValidator
{
    [SerializeField] int m_valueMax = 100;

    public override char Validate(ref string text_, ref int index_, char add_)
    {
        var _null = '\0';

        // 数値か文字かどうかを判定する 数値以外は入力を行わない
        if (false == char.IsDigit(add_))
        {
            return _null;
        }

        // 文字列が空かどうか判定する
        if (text_ == string.Empty)
        {
            text_ += add_;
            index_++;
            return add_;
        }

        // 文字列を数値化
        var _value = int.Parse(text_);
        var _valueMax = m_valueMax;
        var _valueMaxStr = _valueMax.ToString();
        var _str = text_.Insert(index_, add_.ToString());

        // 文字列が既に最大値を上回っている時 最大値に固定
        if (_value > _valueMax)
        {
            text_ = _valueMaxStr;
            return _null;
        }

        if (index_ == (int)Mathf.Log10(_valueMax) + 1)
        {
            text_ = _valueMaxStr;
            return _null;
        }

        if (int.Parse(_str) > _valueMax)
        {
            text_ = _valueMaxStr;
            return _null;
        }

        text_ = _str;
        index_++;

        return add_;
    }
}