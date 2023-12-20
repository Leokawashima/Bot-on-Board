using UnityEngine;
using TMPro;
using AI;

/// <summary>
/// Playerの情報及び付随する情報を表示するクラス
/// </summary>
public class InfoPlayerData : MonoBehaviour
{
    [SerializeField] TMP_Text m_nameText;
    [SerializeField] TMP_Text m_hpText;
    [SerializeField] TMP_Text m_intelligentText;

    public void Initialize(AIAgent ai_)
    {
        name = $"InfoPlayerData_{ai_.Operator.Index}";
        m_nameText.text = $"P{ai_.Operator.Index + 1}";
        Refresh(ai_);
    }

    public void Refresh(AIAgent ai_)
    {
        m_hpText.text = ai_.Health.HP.ToString("F1");
        m_intelligentText.text = ai_.Brain.Intelligent.ToString();
    }
}
