using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanelSystem : MonoBehaviour
{
    [SerializeField] Image m_Image;
    [SerializeField, Range(0, 1)] float m_Value = 0;
    [SerializeField] float m_Time = 1;
    [SerializeField] int m_Cutting = 10;
    [SerializeField] float m_WaitTime = 1;
    public event Action OnFadeComplete;
    public event Action OnFadeEnd;

    public void Enable()
    {
        m_Image.enabled = true;
    }
    public void Disable()
    {
        m_Image.enabled = false;
    }

    public void Fade()
    {
        StartCoroutine(CoFade());
    }

    IEnumerator CoFade()
    {
        Enable();

        float perFadeCut = 1 / (float)m_Cutting;
        float perFadeTime = m_Time / (float)m_Cutting;

        for (int i = 1; i <= m_Cutting; ++i)
        {
            yield return new WaitForSeconds(perFadeTime);
            m_Value = i * perFadeCut;
            m_Image.material.SetFloat("_FadeParam", m_Value);
        }
        m_Image.material.SetFloat("_FadeParam", 1);
        OnFadeComplete?.Invoke();

        yield return new WaitForSeconds(m_WaitTime);

        for (int i = 1; i <= m_Cutting; ++i)
        {
            yield return new WaitForSeconds(perFadeTime);
            m_Value = 1f - i * perFadeCut;
            m_Image.material.SetFloat("_FadeParam", m_Value);
        }
        m_Image.material.SetFloat("_FadeParam", 0);
        OnFadeEnd?.Invoke();

        Disable();
    }

    void OnValidate()
    {
        m_Image.material.SetFloat("_FadeParam", m_Value);
    }
}