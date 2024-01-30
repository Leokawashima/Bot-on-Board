using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Bot;

/// <summary>
/// Playerの情報及び付随する情報を表示するクラス
/// </summary>
public class InfoBotStatus : MonoBehaviour
{
    private BotAgent m_operator;

    [SerializeField] TMP_Text m_nameText;
    [SerializeField] TMP_Text m_hpText;
    [SerializeField] TMP_Text m_intelligentText;
    [SerializeField] Image m_weaponImage;

    private readonly float[] m_COLORS = new float[]
    {
        0.0f,
        240.0f / 360.0f,
        120.0f / 360.0f,
        60.0f / 360.0f,
    };

    public void Initialize(BotAgent bot_)
    {
        m_operator = bot_;

        name = $"InfoBotStatus_{bot_.Operator.Index}";
        m_nameText.text = $"P{bot_.Operator.Index + 1}";
        m_nameText.color = Color.HSVToRGB(m_COLORS[bot_.Operator.Index], 1.0f, 1.0f);
        m_weaponImage.enabled = false;
        Refresh();
    }

    public void Refresh()
    {
        var _bot = m_operator;
        m_hpText.text = _bot.Health.HP.ToString("F1");
        m_intelligentText.text = _bot.Brain.Intelligent.ToString();
        m_weaponImage.enabled = _bot.Assault.Weapon != null;
    }
}
