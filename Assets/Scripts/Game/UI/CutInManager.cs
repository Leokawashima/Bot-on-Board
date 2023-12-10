using System;
using UnityEngine;
using TMPro;

//完成
public class CutInManager : MonoBehaviour
{
    [SerializeField] AnimatorSystem m_AnimatorManager;
    [SerializeField] TextMeshProUGUI m_Text;

    public void CutIn(string text_, Action callback_)
    {
        m_Text.text = text_;
        m_AnimatorManager.Play("CutIn", callback_, 0);
    }

    public void CutIn(string text_)
    {
        m_Text.text = text_;
        m_AnimatorManager.Play("CutIn", 0);
    }
}
