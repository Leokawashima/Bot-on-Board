using UnityEngine;
using System.Collections;
using TMPro;
using AI;

public class DamageUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_text;
    public bool IsUsed = false;

    private Coroutine m_activeCorutine;

    public void Effect(AI.AIAgent ai_, float power_)
    {
        m_activeCorutine = StartCoroutine(CoEffect(ai_, power_));
    }

    private IEnumerator CoEffect(AIAgent ai_, float power_)
    {
        IsUsed = true;
        m_text.gameObject.SetActive(true);
        var _rect = m_text.transform as RectTransform;
        var _pos = ai_.transform.position;

        m_text.text = power_.ToString();

        var _elapsedTime = 0.0f;

        while(_elapsedTime <= 2.0f)
        {
            _elapsedTime += Time.deltaTime;
            _rect.localPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, _pos);
            yield return null;
        }

        m_text.gameObject.SetActive(false);
        IsUsed = false;
        m_activeCorutine = null;
    }
}