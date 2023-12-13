using System;
using UnityEngine;
using TMPro;

/// <summary>
/// カットインに使用するクラス
/// </summary>
public class CutInSystem : MonoBehaviour
{
    [Header("アニメーション名は CutIn で大文字まで統一すること")]
    [SerializeField] AnimatorSystem m_AnimatorSystem;

    [SerializeField] TextMeshProUGUI m_Text;

    public void CutIn(string text_, Action callback_)
    {
        m_Text.text = text_;
        m_AnimatorSystem.Play("CutIn", callback_, 0);
    }

    public void CutIn(string text_)
    {
        m_Text.text = text_;
        m_AnimatorSystem.Play("CutIn", 0);
    }
}