using System;
using System.Collections;
using UnityEngine;
using TMPro;
using AI;

public class FloatingUI :  MonoBehaviour
{
    [SerializeField] float m_timeDestruction = 2.0f;
    [SerializeField] Vector2Int
        m_randomX = new(-100, 100),
        m_randomY = new(0, 100);

    [SerializeField] TMP_Text m_text;
    public event Action<FloatingUI> Event_Finished;

    public void Effect(AIAgent ai_, float power_, Color color_)
    {
        StartCoroutine(CoEffect(ai_, power_, color_));
    }

    private IEnumerator CoEffect(AIAgent ai_, float power_, Color color_)
    {
        var _rect = m_text.transform as RectTransform;
        var _pos = ai_.transform.position;
        var _random = new Vector2(
            UnityEngine.Random.Range(m_randomX.x, m_randomX.y),
            UnityEngine.Random.Range(m_randomY.x, m_randomY.y));

        m_text.text = power_.ToString();
        m_text.color = color_;

        var _elapsedTime = 0.0f;

        while(_elapsedTime <= m_timeDestruction)
        {
            _elapsedTime += Time.deltaTime;
            _rect.localPosition = _random + RectTransformUtility.WorldToScreenPoint(Camera.main, _pos);
            yield return null;
        }

        Event_Finished?.Invoke(this);
    }
}