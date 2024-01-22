using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Imageの反転を行うクラス
/// </summary>
/// 参考　https://hacchi-man.hatenablog.com/entry/2020/03/30/220000
/// ほぼコピペ
[RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
public class FlipImage : UIBehaviour, IMeshModifier
{
    [SerializeField]
    private bool m_isFlipX = false;

    [SerializeField]
    private bool m_isFlipY = false;

    private List<UIVertex> m_uiVertexList = new();
    private RectTransform m_rectTransform;

#if UNITY_EDITOR
    public new void OnValidate()
    {
        GetComponent<Graphic>().SetVerticesDirty();
        Awake();
    }

#endif

    protected override void Awake()
    {
        m_rectTransform = transform as RectTransform;
    }

    public void ModifyMesh(Mesh mesh_)
    {
    }

    public void ModifyMesh(VertexHelper verts_)
    {
        m_uiVertexList.Clear();
        verts_.GetUIVertexStream(m_uiVertexList);

        for (int i = 0, count = m_uiVertexList.Count; i < count; ++i)
        {
            var _vertex = m_uiVertexList[i];
            // pivotの位置によってずらす
            if (m_isFlipX)
            {
                _vertex.position.x += Mathf.Lerp(-m_rectTransform.rect.width, m_rectTransform.rect.width, m_rectTransform.pivot.x);
                _vertex.position.x *= -1;
            }

            if (m_isFlipY)
            {
                _vertex.position.y += Mathf.Lerp(-m_rectTransform.rect.height, m_rectTransform.rect.height, m_rectTransform.pivot.y);
                _vertex.position.y *= -1;
            }

            m_uiVertexList[i] = _vertex;
        }

        verts_.Clear();
        verts_.AddUIVertexTriangleStream(m_uiVertexList);
    }
}