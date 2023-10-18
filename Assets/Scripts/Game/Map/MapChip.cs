using System;
using System.Collections;
using UnityEngine;

public class MapChip : MonoBehaviour
{
    public MapChip_SO_Template m_SO;
    public Vector2Int m_position = Vector2Int.zero;

    Material m_material;
    bool m_isContinue = false;

    void Start()
    {
        m_material = GetComponent<Renderer>().material;
    }

    public void HighLight()
    {
        StartCoroutine(CoHighLight());
    }
    public void Stop()
    {
        m_isContinue = false;
    }

    IEnumerator CoHighLight()
    {
        m_isContinue = true;

        float _h, _s, _v;
        Color.RGBToHSV(m_material.color, out _h,  out _s, out _v);
        var _outLine = m_material.GetColor("_OutlineColor");

        m_material.SetColor("_OutlineColor", Color.blue);

        while (m_isContinue)
        {
            m_material.color = Color.HSVToRGB(_h, Time.time % 1, _v);
            yield return new WaitForSeconds(0.1f);
        }

        m_material.color = Color.HSVToRGB(_h, _s, _v);
        m_material.SetColor("_OutlineColor", _outLine);
    }
}