using UnityEngine;
using TMPro;

/// <summary>
/// AIのHPをUI表示するクラス
/// </summary>
public class AIHPUI : MonoBehaviour
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
