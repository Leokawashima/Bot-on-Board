using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// オブジェクトをアクティブ非アクティブに切り替えるクラス
/// </summary>
public class FlashSystem : MonoBehaviour
{
    /// <summary>
    /// 対象のオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject m_target;

    /// <summary>
    /// 切り替えるカウント数
    /// </summary>
    [SerializeField]
    private int m_count = 4;
    
    /// <summary>
    /// 切り替える時間　秒単位
    /// </summary>
    [SerializeField]
    private float m_time = 1;

    /// <summary>
    /// フラッシュ終了時に呼ばれるコールバック
    /// </summary>
    public event Action OnFinished;

    /// <summary>
    /// コルーチンのアクティブなものを保持するフィールド
    /// </summary>
    private Coroutine m_activeCorutine;

    /// <summary>
    /// フラッシュさせる
    /// </summary>
    public void Flash()
    {
        if (m_activeCorutine == null)
        {
            m_activeCorutine = StartCoroutine(CoFlash());
        }
#if UNITY_EDITOR
        else
        {
            Debug.Log("既にフラッシュを行っているため呼び出しを破棄します");
        }
#endif
    }

    /// <summary>
    /// フラッシュを停止する
    /// </summary>
    public void Stop()
    {
        // ヌルなら例外がスローされるため独自に例外処理を組み込む必要はない
        StopCoroutine(m_activeCorutine);
        m_activeCorutine = null;
    }

    /// <summary>
    /// フラッシュを行うコルーチン
    /// </summary>
    private IEnumerator CoFlash()
    {
        float _perFlashTime = m_time / m_count / 2.0f;
        for (int i = 0; i < m_count; ++i)
        {
            m_target.SetActive(false);
            yield return new WaitForSeconds(_perFlashTime);
            m_target.SetActive(true);
            yield return new WaitForSeconds(_perFlashTime);
        }

        OnFinished?.Invoke();
        m_activeCorutine = null;
    }
}
