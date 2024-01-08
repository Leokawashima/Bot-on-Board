using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

/// <summary>
/// ひらがなカタカナローマ字数字のみ入力できるもの
/// </summary>
/// 参考:https://nekojara.city/unity-input-field-content-type
[CreateAssetMenu(fileName = "NameValidator", menuName = "TextMeshPro/Name Validator")]
public class TMP_NameValidator : TMP_InputValidator
{
    public override char Validate(ref string text_, ref int index_, char add_)
    {
        // 空白なら
        if (char.IsWhiteSpace(add_))
        {
            text_ = text_.Insert(index_, add_.ToString());
            index_++;
            return add_;
        }
        
        // ひらがなかカタカナかローマ字か数字なら
        if (char.IsDigit(add_) || IsHiragana(add_) || IsKatakana(add_) || IsAlpabet(add_))
        {
            text_ = text_.Insert(index_, add_.ToString());
            index_++;
            return add_;
        }

        return add_;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsHiragana(char c_)
    {
        // u3040~u309Fはひらがな
        // u30A1~u30FAはカタカナ
        return c_ >= '\u3040' && c_ <= '\u309F';
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsKatakana(char c_)
    {
        return c_ >= '\u30A1' && c_ <= '\u30FA';
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsAlpabet(char c_)
    {
        return (c_ >= 'A' && c_ <= 'Z') || (c_ >= 'a' && c_ <= 'z');
    }
}