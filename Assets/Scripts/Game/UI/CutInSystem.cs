using System;
using UnityEngine;
using TMPro;

/// <summary>
/// カットインに使用するクラス
/// </summary>
public class CutInSystem : MonoBehaviour
{
    [SerializeField] private Canvas m_canvas;

    [Header("アニメーション名は CutIn で大文字まで統一\n空のデフォルトはSpeed0")]
    [SerializeField] private AnimatorSystem m_animator;

    [SerializeField] private TextMeshProUGUI m_text;

    private int m_cutInHash = Animator.StringToHash("CutIn");

    public void Enable() => m_canvas.enabled = true;
    public void Disable() => m_canvas.enabled = false;

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