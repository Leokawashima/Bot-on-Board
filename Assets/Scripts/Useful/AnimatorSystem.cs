using System;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;

/// <summary>
/// アニメーション再生をラップしたクラス
/// </summary>
public class AnimatorSystem : MonoBehaviour
{
    [Header("TargetAnimator")]
    [SerializeField] private Animator m_animator;

    private int m_defaultHash;

    /// <summary>
    /// デフォルトのアニメーションハッシュを取得する
    /// </summary>
    private void Awake()
    {
        m_defaultHash = m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
    }

    /// <summary>
    /// アニメーション再生を行って終了時にコールバックを呼び出す
    /// </summary>
    /// <param name="name_">再生ステート名</param>
    /// <param name="callback_">再生終了時コールバック</param>
    /// <param name="layer_">再生するレイヤー</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Play(string name_, Action callback_, int layer_ = 0)
    {
        Play(Animator.StringToHash(name_), callback_, layer_);
    }
    /// <summary>
    /// アニメーション再生を行って終了時にコールバックを呼び出す
    /// </summary>
    /// <param name="hash_">再生ステートハッシュ</param>
    /// <param name="callback_">再生終了時コールバック</param>
    /// <param name="layer_">再生するレイヤー</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Play(int hash_, Action callback_, int layer_ = 0)
    {
        m_animator.Play(hash_, layer_);
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
            m_animator.Play(m_defaultHash, 0);
            yield return null;
            callback_();
        }
    }

    /// <summary>
    /// アニメーション再生を行う
    /// </summary>
    /// <param name="name_">再生ステート名</param>
    /// <param name="layer_">再生するレイヤー</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Play(string name_, int layer_ = 0)
    {
        Play(Animator.StringToHash(name_), layer_);
    }
    /// <summary>
    /// アニメーション再生を行う
    /// </summary>
    /// <param name="hash_">再生ステートハッシュ</param>
    /// <param name="layer_">再生するレイヤー</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Play(int hash_, int layer_ = 0)
    {
        m_animator.Play(hash_, layer_);
    }
}