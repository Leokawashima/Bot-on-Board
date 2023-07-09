using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 正の整数のみ入力を許可するためのクラス
/// </summary>
/// 参考:https://nekojara.city/unity-input-field-content-type
public class RoomPassward : MonoBehaviour
{
    [SerializeField] TMP_InputField input;

    void Awake()
    {
        input.onValidateInput += OnPasswardInputField;
    }

    char OnPasswardInputField(string text_, int index_, char add_)
    {
        if(!char.IsDigit(add_)) return '\0';
        return add_;
    }
}
