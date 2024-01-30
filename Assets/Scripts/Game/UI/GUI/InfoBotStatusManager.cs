using UnityEngine;
using Bot;

/// <summary>
/// Player等の情報を表示するクラスの管理クラス
/// </summary>
public class InfoBotStatusManager : MonoBehaviour
{
    [SerializeField] InfoBotStatus m_prefab;
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    private InfoBotStatus[] m_statuses;

    public void Initialize()
    {
        var _aiList = BotManager.Singleton.Bots;
        m_statuses = new InfoBotStatus[_aiList.Count];
        for (int i = 0, len = m_statuses.Length; i < len; ++i)
        {
            var _ai = _aiList[i];
            m_statuses[i] = Instantiate(m_prefab, transform);
            m_statuses[i].Initialize(_ai);
        }
    }

    public void Refresh(int index_)
    {
        m_statuses[index_].Refresh();
    }
}
