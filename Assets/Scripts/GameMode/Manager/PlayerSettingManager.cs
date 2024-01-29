using UnityEngine;
using TMPro;

public class PlayerSettingManager : SingletonMonoBehaviour<PlayerSettingManager>
{
    [SerializeField] private Canvas m_canvas;

    private InfoPlayerSetting m_target;

    [SerializeField] private TMP_Text m_indexText;
    [SerializeField] private TMP_InputField m_nameInputField;

    [field: SerializeField] public PlusMinusButton BotPMButton { get; private set; }

    public void Enable() => m_canvas.enabled = true;
    public void Disable() => m_canvas.enabled = false;

    public void Initialize()
    {
        m_nameInputField.onEndEdit.AddListener(OnEndEdit);

        BotPMButton.Event_ValueAdd += OnAddValue;
        BotPMButton.Event_ValueSub += OnSubValue;
    }
    public void Select(InfoPlayerSetting info_)
    {
        m_target = info_;
        m_indexText.text = (info_.Data.Index + 1).ToString();
        m_nameInputField.text = info_.Data.Name.ToString();
        BotPMButton.SetValue(info_.Data.BotOperations);
    }
    private void OnEndEdit(string text_)
    {
        if (m_target == null) return;

        m_target.Data.SetName(text_);
        m_target.Set(m_target.Data);
    }
    void OnAddValue(int value_)
    {
        if (m_target == null) return;

        m_target.Data.AddBot(value_);
    }
    void OnSubValue(int value_)
    {
        if (m_target == null) return;

        m_target.Data.SubBot(value_);
    }
}