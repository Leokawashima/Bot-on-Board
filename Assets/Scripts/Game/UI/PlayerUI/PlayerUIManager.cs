using System;
using UnityEngine;
using Game;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private PlayerUI m_prefab;

#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    private PlayerUI[] m_playerUIArray;

    public static event Action
        Event_ButtonPlace,
        Event_ButtonTurnEnd;

    public void Initialize()
    {
        m_playerUIArray = new PlayerUI[GameManager.PLAYER_SIZE];
        for (int i = 0, len = m_playerUIArray.Length; i < len; ++i)
        {
            m_playerUIArray[i] = Instantiate(m_prefab, transform);
            m_playerUIArray[i].Initialize();
            m_playerUIArray[i].Event_ButtonPlace += () =>
            {
                Event_ButtonPlace?.Invoke();
            };
            m_playerUIArray[i].Event_ButtonTurnEnd += () =>
            {
                Event_ButtonTurnEnd?.Invoke();
            };
            m_playerUIArray[i].gameObject.SetActive(false);
        }
    }

    public void TurnInitialize()
    {
        foreach (var ui_ in m_playerUIArray)
        {
            ui_.TurnInitialize();
        }
    }

    public void TurnPlace()
    {
        m_playerUIArray[GameManager.Singleton.ProcessingPlayerIndex].gameObject.SetActive(true);
    }
}