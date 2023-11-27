using System;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditManager : MonoBehaviour
{
    [SerializeField] DeckManager m_deckManager;

    [SerializeField] DeckEditArea m_editArea;
    [SerializeField] DeckListInfo m_listInfo;

    [SerializeField] private Button m_backButton;
    [SerializeField] private Button m_saveButton;

    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        DeckManager.Event_Initialize += Initialize;
    }
    private void OnDestroy()
    {
        DeckManager.Event_Initialize -= Initialize;
    }

    private void Initialize()
    {
        Disable();

        m_backButton.onClick.AddListener(() =>
        {
            Disable();
            m_deckManager.DeckList.Enable();
        });
        m_saveButton.onClick.AddListener(() =>
        {
            m_deckManager.DeckList.SelectInfo.Data.Name = "New Save";
            m_deckManager.DeckList.SelectInfo.Data.CardIndexArray = new int[10];
            for (int i = 0; i < m_editArea.EditCardList.Length; ++i)
            {
                m_deckManager.DeckList.SelectInfo.Data.CardIndexArray[i] = m_editArea.EditCardList[i].m_Index;
            }
            m_deckManager.DeckList.Save(m_deckManager.DeckList.SelectInfo.Index, m_deckManager.DeckList.SelectInfo.Data);
            m_deckManager.DeckList.SelectInfo.NameText = "New Save";

            Disable();
            m_deckManager.DeckList.Enable();
        });
    }
}