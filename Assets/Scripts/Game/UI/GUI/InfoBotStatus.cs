using UnityEngine;
using TMPro;
using Bot;

/// <summary>
/// Playerの情報及び付随する情報を表示するクラス
/// </summary>
public class InfoBotStatus : MonoBehaviour
{
    [SerializeField] TMP_Text m_nameText;
    [SerializeField] TMP_Text m_hpText;
    [SerializeField] TMP_Text m_intelligentText;

    public void Initialize(BotAgent bot_)
    {
        name = $"InfoBotStatus_{bot_.Operator.Index}";
        m_nameText.text = $"P{bot_.Operator.Index + 1}";
        Refresh(bot_);
    }

    public void Refresh(BotAgent bot_)
    {
        m_hpText.text = bot_.Health.HP.ToString("F1");
        m_intelligentText.text = bot_.Brain.Intelligent.ToString();
    }
}
