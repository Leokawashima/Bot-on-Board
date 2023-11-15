using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Imageをフェードさせるクラス
/// </summary>
public class FadePanelSystem : MonoBehaviour
{
    /// <summary>
    /// フェードさせる画像
    /// </summary>
    [SerializeField]
    private Image m_image;

    /// <summary>
    /// フェードさせる全体の時間
    /// </summary>
    [SerializeField] float m_wholeTime = 1;

    /// <summary>
    /// 画像を変化させる回数
    /// </summary>
    [SerializeField] int m_cutting = 10;

    /// <summary>
    /// フェードインからフェードアウトまでの間の秒数
    /// </summary>
    [SerializeField] float m_waitTime = 1;

    /// <summary>
    /// // フェードの度合い
    /// </summary>
#if UNITY_EDITOR
    [Header("Debug"), SerializeField, Range(0, 1),]
#endif
    float m_fadeValue = 0;

    /// <summary>
    /// フェードイン終了時のコールバック
    /// </summary>
    public event Action OnFadeInCompleted;

    /// <summary>
    /// フェードアウト終了時のコールバック
    /// </summary>
    public event Action OnFadeFinished;

    /// <summary>
    /// 変化させるマテリアルのパラメータ　専用のシェーダにあるパラメータとなる
    /// </summary>
    private readonly int PARAMETER = Shader.PropertyToID("_FadeParam");

    /// <summary>
    /// 画像の有効化
    /// </summary>
    public void Enable()
    {
        m_image.enabled = true;
    }

    /// <summary>
    /// 画像の無効化
    /// </summary>
    public void Disable()
    {
        m_image.enabled = false;
    }

    /// <summary>
    /// フェードさせる
    /// </summary>
    public void Fade()
    {
        StartCoroutine(CoFade());
    }

    /// <summary>
    /// フェードを行うコルーチン
    /// </summary>
    IEnumerator CoFade()
    {
        Enable();

        float
            _perFadeCutting = 1 / (float)m_cutting,
            _perFadeTime = m_wholeTime / m_cutting;

        for (int i = 1; i <= m_cutting; ++i)
        {
            yield return new WaitForSeconds(_perFadeTime);
            m_fadeValue = i * _perFadeCutting;
            m_image.material.SetFloat(PARAMETER, m_fadeValue);
        }
        m_image.material.SetFloat(PARAMETER, 1);
        OnFadeInCompleted?.Invoke();

        yield return new WaitForSeconds(m_waitTime);

        for (int i = 1; i <= m_cutting; ++i)
        {
            yield return new WaitForSeconds(_perFadeTime);
            m_fadeValue = 1f - i * _perFadeCutting;
            m_image.material.SetFloat(PARAMETER, m_fadeValue);
        }
        m_image.material.SetFloat(PARAMETER, 0);
        OnFadeFinished?.Invoke();

        Disable();
    }

#if UNITY_EDITOR
    /// <summary>
    /// デバッグ用に値を変更してフェードの度合いをチェックする
    /// </summary>
    void OnValidate()
    {
        m_image.material.SetFloat(PARAMETER, m_fadeValue);
    }
#endif
}