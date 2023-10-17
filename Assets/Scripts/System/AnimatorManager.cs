using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// アニメーション再生をコーディングしたクラス
/// </summary>
public class AnimatorManager : MonoBehaviour
{
    [Header("TargetAnimator")]
    [SerializeField] Animator m_animator;
    [Header("Setting")]
    [SerializeField] string m_defaultStateName = "New State";

    /// <summary>
    /// アニメーション再生を行って終了時にコールバックを呼び出す
    /// </summary>
    /// <param name="name_">再生ステート名</param>
    /// <param name="callback_">再生終了時コールバック</param>
    /// <param name="layer_">再生するレイヤー</param>
    public void Play(string name_, Action callback_, int layer_ = 0)
    {
        m_animator.Play(name_, layer_);
        // asyncは優秀な反面マルチスレッドやMonoBehaiviourに完全に紐づけられているわけではないので
        // コルーチンでのコールバック呼び出し実装を行っている
        StartCoroutine(Co_AnimatorCallBack());
        
        IEnumerator Co_AnimatorCallBack()
        {
            // アニメーションの再生時間の割合0~1で1以下ならアニメーション再生中
            while (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            /*
             * Animator君はMakeTransitionを条件なしで動かすと
             * normalizeTimeが1を超えないでステートが切り替わることがある
             * 要するにアニメーションが終了したかを判定できずに勝手に遷移させてしまう
             * なのでdefaultステートの名前をEmptyに固定、スピードは0で
             * 遷移を完全にスクリプトベースで操作することで終了コールバックを実現している
             */
            m_animator.Play(m_defaultStateName, 0);
            callback_();
        }
    }

    /// <summary>
    /// アニメーション再生を行う
    /// </summary>
    /// <param name="name_">再生ステート名</param>
    /// <param name="layer_">再生するレイヤー</param>
    public void Play(string name_, int layer_ = 0)
    {
        m_animator.Play(name_, layer_);
    }
}