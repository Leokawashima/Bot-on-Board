using System;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private PlayerUI m_prefab;

#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    PlayerUI[] m_PlayerUIArray;
    public static event Action
        Event_ButtonPlace,
        Event_ButtonTurnEnd;

    public void Initialize()
    {
        //ローカルの場合は人数分
        m_PlayerUIArray = new PlayerUI[2];
        for (int i = 0; i < m_PlayerUIArray.Length; ++i)
        {
            m_PlayerUIArray[i] = Instantiate(m_prefab, transform);
            m_PlayerUIArray[i].Initialize();
            m_PlayerUIArray[i].Event_ButtonPlace += () =>
            {
                Event_ButtonPlace?.Invoke();
            };
            m_PlayerUIArray[i].Event_ButtonTurnEnd += () =>
            {
                Event_ButtonTurnEnd?.Invoke();
            };
            m_PlayerUIArray[i].gameObject.SetActive(false);
        }
    }

    public void TurnInitialize()
    {
        foreach (var ui_ in m_PlayerUIArray)
        {
            ui_.TurnInitialize();
        }
    }

    public void TurnPlace()
    {
        m_PlayerUIArray[GameManager.Singleton.PlayerIndex].gameObject.SetActive(true);
    }
}