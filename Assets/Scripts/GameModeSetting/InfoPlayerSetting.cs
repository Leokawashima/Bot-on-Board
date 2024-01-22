using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPlayerSetting : MonoBehaviour
{
    public PlayerSetting Data { get; private set; }
    [SerializeField] private Button m_button;

    [SerializeField] private Shadow m_shadow;
    [SerializeField] private TMP_Text m_indexText;
    [SerializeField] private TMP_Text m_nameText;

    public Action<InfoPlayerSetting> Event_OnClick;

    public void Initlaize(PlayerSetting data_)
    {
        Data = data_;
        Set(data_.Index, data_.HSVColor, data_.Name);
        m_button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Event_OnClick?.Invoke(this);
    }

    public void Set(int index_, float color_, string name_)
    {
        var _textColor = Color.HSVToRGB(color_, 0.7f, 1.0f);
        m_indexText.color = _textColor;
        m_indexText.text = $"Player{index_ + 1}";

        var _shadowColor = Color.HSVToRGB(color_, 1.0f, 1.0f);
        _shadowColor.a -= 0.5f;
        m_shadow.effectColor = _shadowColor;

        m_nameText.text = name_;
    }
}