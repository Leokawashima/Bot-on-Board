using UnityEngine;
using TMPro;

public class PlayerSettingManager : SingletonMonoBehaviour<PlayerSettingManager>
{
    [SerializeField] private Canvas m_canvas;

    private InfoPlayerSetting m_target;

    [SerializeField] private TMP_Text m_indexText;
    [SerializeField] private TMP_InputField m_nameInputField;

    [field: SerializeField] public PlusMinusButton BotPMButton { get; private set; }

    private readonly float[] m_COLORS = new float[]
    {
        0.0f,
        240.0f / 360.0f,
        120.0f / 360.0f,
        60.0f / 360.0f,
    };

    public void Enable() => m_canvas.enabled = true;
    public void Disable() => m_canvas.enabled = false;

    public void Initialize()
    {
        m_nameInputField.onEndEdit.AddListener(OnEndEdit);

        BotPMButton.Event_ValueAdd += OnValueAdd;
        BotPMButton.Event_ValueSub += OnValueSub;
    }
    public void Select(InfoPlayerSetting info_)
    {
        m_target = info_;
        m_indexText.text = $"Player{info_.Data.Index + 1}";
        m_indexText.color = Color.HSVToRGB(m_COLORS[info_.Data.Index], 1.0f, 1.0f);
        m_nameInputField.text = info_.Data.Name.ToString();
        BotPMButton.SetValue(info_.Data.BotSettings.Count);
        BotSettingManager.Singleton.Select(info_.Data.BotSettings);
    }
    private void OnEndEdit(string text_)
    {
        if (m_target == null) return;

        m_target.Data.SetName(text_);
        m_target.Set(m_target.Data);
    }
    private void OnValueAdd(int value_)
    {
        if (m_target == null) return;

        m_target.Data.AddBot(value_);
    }
    private void OnValueSub(int value_)
    {
        if (m_target == null) return;

        m_target.Data.SubBot(value_);
    }
}