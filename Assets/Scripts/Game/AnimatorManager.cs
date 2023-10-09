using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField] Animator m_Animator;
    
    /// <summary>
    /// アニメーション再生を行って終了時にコールバックを呼び出す
    /// </summary>
    /// <param name="name_">再生ステート名</param>
    /// <param name="callback_">コールバック</param>
    public void Play(string name_, Action callback_, int layer_ = 0)
    {
        m_Animator.Play(name_, layer_);
        IEnumerator Co()
        {
            while(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
            {
				yield return null;
            }
            callback_();
        }
        StartCoroutine(Co());
    }

    public void Play(string name_, int layer_ = 0)
    {
        m_Animator.Play(name_, layer_);
    }
}