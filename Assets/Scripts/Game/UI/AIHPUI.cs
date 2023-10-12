using UnityEngine;
using TMPro;

//完成
public class AIHPUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_NameText;
    [SerializeField] TextMeshProUGUI m_HPText;

    public void Initialize(string name_)
    {
        m_NameText.text = name_;
    }

    public void SetHP(float hp_)
    {
        m_HPText.text = hp_.ToString("F1");
    }
}
