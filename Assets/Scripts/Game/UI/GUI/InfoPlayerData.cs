using UnityEngine;
using TMPro;

/// <summary>
/// Playerの情報及び付随する情報を表示するクラス
/// </summary>
public class InfoPlayerData : MonoBehaviour
{
    [SerializeField] TMP_Text m_nameText;
    [SerializeField] TMP_Text m_hpText;

    public void Initialize(string name_)
    {
        m_nameText.text = name_;
    }

    public void SetHP(float hp_)
    {
        m_hpText.text = hp_.ToString("F1");
    }
}
