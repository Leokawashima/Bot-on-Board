using System.Collections;
using UnityEngine;

/// <summary>
/// オブジェクトをアクティブ非アクティブに切り替えるクラス
/// </summary>
public class FlashSystem : CorutineMonoBehaivour
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
    /// フラッシュを行うコルーチン
    /// </summary>
    protected override IEnumerator CoProcess()
    {
        float _perFlashTime = m_time / m_count / 2.0f;
        for (int i = 0; i < m_count; ++i)
        {
            m_target.SetActive(false);
            yield return new WaitForSeconds(_perFlashTime);
            m_target.SetActive(true);
            yield return new WaitForSeconds(_perFlashTime);
        }
    }
}
