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
            var _deckData = new DeckData()
            {
                Name = "New Save",
                CardIndexArray = new int[m_editArea.EditCardList.Length],


            };
            for (int i = 0; i < m_editArea.EditCardList.Length; ++i)
            {
                _deckData.CardIndexArray[i] = m_editArea.EditCardList[i].Index;
            }
            m_deckManager.DeckList.Save(m_deckManager.DeckList.SelectInfo.Index, _deckData);
            m_deckManager.DeckList.SelectInfo.SetData(_deckData);
            m_deckManager.DeckList.SelectInfo.ReFresh();
            m_listInfo.SetInfo(m_deckManager.DeckList.SelectInfo);

            Disable();
            m_deckManager.DeckList.Enable();
        });
    }
}