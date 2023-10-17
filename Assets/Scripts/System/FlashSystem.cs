using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSystem : MonoBehaviour
{
    [SerializeField] GameObject m_Target;
    [SerializeField] int m_Cnt = 4;
    [SerializeField] float m_Time = 1;

    public event Action OnFlashFinished;
    
    public void Flash()
    {
        StartCoroutine(CoFlash());
    }

    IEnumerator CoFlash()
    {
        float perFlashTime = m_Time / (float)m_Cnt / 2.0f;
        for (int i = 0; i < m_Cnt; ++i)
        {
            m_Target.SetActive(true);
            yield return new WaitForSeconds(perFlashTime);
            m_Target.SetActive(false);
            yield return new WaitForSeconds(perFlashTime);
        }

        OnFlashFinished?.Invoke();
    }
}
