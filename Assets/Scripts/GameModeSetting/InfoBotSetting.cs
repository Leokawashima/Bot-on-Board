using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBotSetting : MonoBehaviour
{
    public BotSetting Data { get; private set; }
    [SerializeField] private Button m_button;

    [SerializeField] private TMP_Text m_indexText;
    [SerializeField] private TMP_Text m_settingText;

    public Action<InfoBotSetting> Event_Click;

    public void Initialize(BotSetting data_)
    {
        Data = data_;
        Set(data_);
        m_button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Event_Click?.Invoke(this);
    }

    public void Set(BotSetting data_)
    {
        m_indexText.text = $"Bot{data_.Index + 1}";
        m_settingText.text = $"{data_.HP} {data_.HPMax} {data_.Attack}";
    }
}