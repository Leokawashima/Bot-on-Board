using System;
using System.Collections;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField] Animator m_animator;
    
    /// <summary>
    /// アニメーション再生を行って終了時にコールバックを呼び出す
    /// </summary>
    /// <param name="name_">再生ステート名</param>
    /// <param name="callback_">コールバック</param>
    public void Play(string name_, Action callback_, int layer_ = 0)
    {
        m_animator.Play(name_, layer_);
        IEnumerator Co()
        {
            /*
             * 一フレーム飛ばさないと正常にステート取得が行えない場合がある
             * Animator内の設定次第な気がする…が終了コールバックを呼んであげる処理なので
             * do whileで明示的に先に1フレーム飛ばしてから条件チェックしている
             */
            do
            {
                yield return null;
            }
            while (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

            callback_();
        }
        StartCoroutine(Co());
    }

    public void Play(string name_, int layer_ = 0)
    {
        m_animator.Play(name_, layer_);
    }
}