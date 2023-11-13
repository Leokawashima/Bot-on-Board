using System.Collections;
using UnityEngine;
using TMPro;

public class RainbowTextSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_Text;
    [SerializeField] Color m_StartColor = Color.red;
    [SerializeField] float m_Speed = 1.0f;

    private void Start()
    {
        Rainbow();
    }

    public void Rainbow()
    {
        StartCoroutine(CoRainbow());
    }

    IEnumerator CoRainbow()
    {
        m_Text.color = m_StartColor;
        yield return null;

        while (true)
        {
            m_Text.color = Color.HSVToRGB(Time.time * m_Speed % 1.0f, 1.0f, 1.0f);
            yield return null;
        }
    }
}