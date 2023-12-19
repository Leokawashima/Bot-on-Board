using UnityEngine;
using AI;

/// <summary>
/// Player等の情報を表示するクラスの管理クラス
/// </summary>
public class InfoPlayerDataManager : MonoBehaviour
{
    [SerializeField] InfoPlayerData m_prefab;
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    private InfoPlayerData[] m_infoArray;

    public void Initialize()
    {
        var _aiList = AIManager.Singleton.AIList;
        m_infoArray = new InfoPlayerData[_aiList.Count];
        for (int i = 0, len = m_infoArray.Length; i < len; ++i)
        {
            var _ai = _aiList[i];
            m_infoArray[i] = Instantiate(m_prefab, transform);
            m_infoArray[i].name = $"InfoPlayerData_{_ai.Operator.Name}";
            m_infoArray[i].Initialize($"P{_ai.Operator.Index + 1}");
            m_infoArray[i].SetHP(_ai.State.HP);
        }
    }

    public void Refresh(int index_, float hp_)
    {
        m_infoArray[index_].SetHP(hp_);
    }
}
