using System.Collections.Generic;
using UnityEngine;

public class AIHPUIManager : MonoBehaviour
{
    [SerializeField] AIHPUI m_AIHPUI;
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    private AIHPUI[] m_AIHPUIArray;

    public void Initialize(List<AISystem> ai_)
    {
        m_AIHPUIArray = new AIHPUI[ai_.Count];
        for (int i = 0; i < ai_.Count; ++i)
        {
            m_AIHPUIArray[i] = Instantiate(m_AIHPUI, transform);
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
