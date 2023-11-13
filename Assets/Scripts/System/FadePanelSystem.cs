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

    private readonly int PARAMETER = Shader.PropertyToID("_FadeParam");

    public void Enable()
    {
        m_image.enabled = true;
    }
    public void Disable()
    {
        m_image.enabled = false;
    }

    public void Fade()
    {
        StartCoroutine(Co_Fade());

        IEnumerator Co_Fade()
        {
            Enable();

            float
                _perFadeCut = 1 / (float)m_cutting,
                _perFadeTime = m_wholeTime / m_cutting;

            for(int i = 1; i <= m_cutting; ++i)
            {
                yield return new WaitForSeconds(_perFadeTime);
                m_fadeValue = i * _perFadeCut;
                m_image.material.SetFloat(PARAMETER, m_fadeValue);
            }
            m_image.material.SetFloat(PARAMETER, 1);
            OnFadeInCompleted?.Invoke();

            yield return new WaitForSeconds(m_waitTime);

            for(int i = 1; i <= m_cutting; ++i)
            {
                yield return new WaitForSeconds(_perFadeTime);
                m_fadeValue = 1f - i * _perFadeCut;
                m_image.material.SetFloat(PARAMETER, m_fadeValue);
            }
            m_image.material.SetFloat(PARAMETER, 0);
            OnFadeFinished?.Invoke();

            Disable();
        }
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        m_image.material.SetFloat(PARAMETER, m_fadeValue);
    }
#endif
}