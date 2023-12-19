using System;
using System.Collections;
using UnityEngine;

public abstract class CorutineMonoBehaivour : MonoBehaviour
{
    /// <summary>
    /// コルーチンのアクティブなものを保持するフィールド
    /// </summary>
    private Coroutine m_activeCorutine;

    public event Action Event_Finished;

    protected abstract IEnumerator CoProcess();

    public void Active()
    {
        if (m_activeCorutine == null)
        {
            m_activeCorutine = StartCoroutine(CoEvent());
        }
    }
    private IEnumerator CoEvent()
    {
        yield return CoProcess();
        m_activeCorutine = null;
        Event_Finished?.Invoke();
    }

    public void Stop()
    {
        if (m_activeCorutine != null)
        {
            StopCoroutine(m_activeCorutine);
            m_activeCorutine = null;
        }
    }
}