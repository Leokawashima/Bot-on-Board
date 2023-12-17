using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AIへの作戦UIを管理するクラス
/// </summary>
public class InfoAIOrderManager : MonoBehaviour
{
    [SerializeField] private ToggleGroup m_toggleGroup;
    [SerializeField] private InfoAIOrder m_prefab;

    readonly string[] m_OrderStr =
        {
        "通常",
        "ガンガン攻めろ",
        "命を大事に"
    };

#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    private InfoAIOrder[] m_AIOrderArray;

    public void Initialize()
    {
        m_AIOrderArray = new InfoAIOrder[m_OrderStr.Length];
        for (int i = 0, len = m_AIOrderArray.Length; i < len; ++i)
        {
            m_AIOrderArray[i] = Instantiate(m_prefab, transform);
            m_AIOrderArray[i].Initialize(m_OrderStr[i], i, m_toggleGroup);
        }
    }
}
