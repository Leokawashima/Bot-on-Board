using System;
using UnityEngine;
using TMPro;

/// <summary>
/// カットインに使用するクラス
/// </summary>
public class CutInSystem : MonoBehaviour
{
    [Header("アニメーション名は CutIn で大文字まで統一\n空のデフォルトはSpeed0")]
    [SerializeField] private AnimatorSystem m_animator;

    [SerializeField] private TextMeshProUGUI m_text;

    private int m_cutInHash = Animator.StringToHash("CutIn");

    public void Play(string text_, Action callback_)
    {
        m_text.text = text_;
        m_animator.Play(m_cutInHash, callback_, 0);
    }

    public void Play(string text_)
    {
        m_text.text = text_;
        m_animator.Play(m_cutInHash, 0);
    }
}