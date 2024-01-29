using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPlayerSetting : MonoBehaviour
{
    [field: SerializeField] public PlayerSetting Data { get; private set; }
    [SerializeField] private Button m_button;

    [SerializeField] private Shadow m_shadow;
    [SerializeField] private TMP_Text m_indexText;
    [SerializeField] private TMP_Text m_nameText;

    public Action<InfoPlayerSetting> Event_Click;

    public void Initlaize(PlayerSetting data_)
    {
        Data = data_;
        Set(data_);
        m_button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Event_Click?.Invoke(this);
    }

    public void Set(PlayerSetting data_)
    {
        var _textColor = Color.HSVToRGB(data_.HSVColor, 0.7f, 1.0f);
        m_indexText.color = _textColor;
        m_indexText.text = $"Player{data_.Index + 1}";

        var _shadowColor = Color.HSVToRGB(data_.HSVColor, 1.0f, 1.0f);
        _shadowColor.a -= 0.5f;
        m_shadow.effectColor = _shadowColor;

        m_nameText.text = data_.Name;
    }
}