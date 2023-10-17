using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Imageをフェードさせるクラス
/// </summary>
public class FadePanelSystem : MonoBehaviour
{
    [SerializeField] Image m_image;                     // フェードさせる画像
    [SerializeField, Range(0, 1),] float m_value = 0;   // フェードの度合い
    [SerializeField] float m_wholeTime = 1;             // フェードさせる全体の時間
    [SerializeField] int m_cutting = 10;                // 画像を変化させる回数
    [SerializeField] float m_waitTime = 1;              // フェードインからフェードアウトまでのかかる秒数
    public event Action OnFadeInCompleted;              // フェードイン終了時のコールバック
    public event Action OnFadeFinished;                 // フェードアウト終了時のコールバック

    const string PARAMETER = "_FadeParam";

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

            float _perFadeCut = 1 / (float)m_cutting;
            float _perFadeTime = m_wholeTime / (float)m_cutting;

            for(int i = 1; i <= m_cutting; ++i)
            {
                yield return new WaitForSeconds(_perFadeTime);
                m_value = i * _perFadeCut;
                m_image.material.SetFloat(PARAMETER, m_value);
            }
            m_image.material.SetFloat(PARAMETER, 1);
            OnFadeInCompleted?.Invoke();

            yield return new WaitForSeconds(m_waitTime);

            for(int i = 1; i <= m_cutting; ++i)
            {
                yield return new WaitForSeconds(_perFadeTime);
                m_value = 1f - i * _perFadeCut;
                m_image.material.SetFloat(PARAMETER, m_value);
            }
            m_image.material.SetFloat(PARAMETER, 0);
            OnFadeFinished?.Invoke();

            Disable();
        }
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        m_image.material.SetFloat(PARAMETER, m_value);
    }
#endif
}