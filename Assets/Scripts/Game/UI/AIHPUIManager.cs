using UnityEngine;

//完成
public class AIHPUIManager : MonoBehaviour
{
    [SerializeField] AIHPUI m_AIHPUI;
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    AIHPUI[] m_AIHPUIArray;

    public void Initialize(int players_, float hp_)
    {
        m_AIHPUIArray = new AIHPUI[players_];
        for (int i = 0; i < players_; ++i)
        {
            m_AIHPUIArray[i] = Instantiate(m_AIHPUI, transform);
            m_AIHPUIArray[i].name = $"AIHPUI_Player_{i + 1}";
            m_AIHPUIArray[i].Initialize($"P{i + 1}");
            m_AIHPUIArray[i].SetHP(hp_);
        }
    }

    public void Refresh(int index_, float hp_)
    {
        m_AIHPUIArray[index_].SetHP(hp_);
    }
}
