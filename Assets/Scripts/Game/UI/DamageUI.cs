using UnityEngine;
using System.Collections;
using TMPro;

public class DamageUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_text;
    public bool IsUsed = false;

    private Coroutine m_activeCorutine;

    public void Effect(AISystem ai_, float power_)
    {
        m_activeCorutine = StartCoroutine(CoEffect(ai_, power_));
    }

    private IEnumerator CoEffect(AISystem ai_, float power_)
    {
        IsUsed = true;
        m_text.gameObject.SetActive(true);
        var _rect = m_text.transform as RectTransform;
        _rect.localPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, ai_.transform.position) - new Vector2(1920 / 2.0f, 1080 / 2.0f);

        m_text.text = power_.ToString();
        yield return new WaitForSeconds(2.0f);

        m_text.gameObject.SetActive(false);
        IsUsed = false;
        m_activeCorutine = null;
    }
}