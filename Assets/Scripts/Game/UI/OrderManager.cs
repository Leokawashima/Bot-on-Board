using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AIへの作戦を管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class OrderManager : MonoBehaviour
{
    [SerializeField] ToggleGroup m_ToggleGroup;
    [SerializeField] AIOrder m_AIOrder;
    readonly string[] m_OrderStr = { "通常", "ガンガン攻めろ", "命を大事に" };

#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    AIOrder[] m_AIOrderArray;

    public void Initialize()
    {
        m_AIOrderArray = new AIOrder[m_OrderStr.Length];
        for(int i = 0; i < m_AIOrderArray.Length; ++i)
        {
            m_AIOrderArray[i] = Instantiate(m_AIOrder, transform);
            m_AIOrderArray[i].Initialize(m_OrderStr[i], i, m_ToggleGroup);
        }
    }
}
