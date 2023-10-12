using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChip : MonoBehaviour
{
    public MapChip_SO_Template m_SO;
    public Vector2Int m_Pos = Vector2Int.zero;

    Material m_Material;
    bool m_Flag = false;

    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }

    public void HighLight()
    {
        StartCoroutine(CoHighLight());
    }
    public void Stop()
    {
        m_Flag = false;
    }

    IEnumerator CoHighLight()
    {
        m_Flag = true;
        float h, s, v;
        Color.RGBToHSV(m_Material.color, out h,  out s, out v);
        var outline = m_Material.GetColor("_OutlineColor");

        while(m_Flag)
        {
            m_Material.color = Color.HSVToRGB(h, Time.time % 1, v);
            m_Material.SetColor("_OutlineColor", Color.blue);
            yield return new WaitForSeconds(0.1f);
        }

        m_Material.color = Color.HSVToRGB(h, s, v);
        m_Material.SetColor("_OutlineColor", outline);
    }
}