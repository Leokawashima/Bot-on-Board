using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player等の情報を表示するクラスの管理クラス
/// </summary>
public class InfoPlayerDataManager : MonoBehaviour
{
    [SerializeField] InfoPlayerData m_prefab;
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    private InfoPlayerData[] m_AIHPUIArray;

    public void Initialize(List<AISystem> ai_)
    {
        m_AIHPUIArray = new InfoPlayerData[ai_.Count];
        for (int i = 0; i < ai_.Count; ++i)
        {
            m_AIHPUIArray[i] = Instantiate(m_prefab, transform);
            m_AIHPUIArray[i].name = $"AIHPUI_Player_{i + 1}";
            m_AIHPUIArray[i].Initialize($"P{i + 1}");
            m_AIHPUIArray[i].SetHP(ai_[i].m_HP);
        }
    }

    public void Refresh(int index_, float hp_)
    {
        m_AIHPUIArray[index_].SetHP(hp_);
    }
}
