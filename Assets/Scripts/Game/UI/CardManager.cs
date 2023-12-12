using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private const int
        DECK_SIZE = 10,
        HAND_SIZE = 4,
        DRAW_SIZE = 2;

    private const float CARD_SELECT_OFFSET = 50.0f;

    [SerializeField] private CardGenerator m_cardGenerator;

    public MapObjectCard GetSelectCard
    {
        get
        {
            return m_toggleGroup.ActiveToggles().FirstOrDefault().
                GetComponent<MapObjectCard>();
        }
    }

    [SerializeField] List<int> m_deck = new(DECK_SIZE);

    [SerializeField] private ToggleGroup m_toggleGroup;

#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<int> m_trashCardList = new();

#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<int> m_handCardList = new();
    public List<int> HandCard => m_handCardList;

#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<int> m_stockCardList = new();

    public void Initialize()
    {
        for (int i = 0; i < HAND_SIZE; ++i)
        {
            var _index = Random.Range(0, m_deck.Count - 1);

            m_handCardList.Add(m_deck[_index]);

            CardCreate(m_deck[_index]);

            m_deck.RemoveAt(_index);
        }
        m_stockCardList = new(m_deck);
        m_deck.Clear();
    }

    private void CardCreate(int index_)
    {
        var _moc = m_cardGenerator.Create(index_, transform);

        var _toggle = _moc.gameObject.GetComponent<Toggle>();
        _toggle.group = m_toggleGroup;

        _moc.Event_Trash += () =>
        {
            m_trashCardList.Add(index_);
            m_handCardList.Remove(index_);
        };

        var _rect = _moc.transform as RectTransform;
        _rect.localScale = Vector2.one * 0.5f;

        _toggle.onValueChanged.AddListener((bool isOn_) =>
        {
            _rect.anchoredPosition = new Vector2(
                _rect.anchoredPosition.x,
                isOn_ ?
                _rect.anchoredPosition.y + CARD_SELECT_OFFSET : _rect.anchoredPosition.y - CARD_SELECT_OFFSET
                );
        });
    }
    
    public void Draw()
    {
        var _space = HAND_SIZE - m_handCardList.Count;
        var _draw = Mathf.Min(_space, DRAW_SIZE);

        for (int i = 0; i < _draw; ++i)
        {
            var _index = Random.Range(0, m_stockCardList.Count - 1);
            m_handCardList.Add(m_stockCardList[_index]);

            CardCreate(m_stockCardList[_index]);

            m_stockCardList.RemoveAt(_index);
        }

        //ドローが終わったら破棄カードたちを山札に戻す
        foreach (var index in m_trashCardList)
            m_stockCardList.Add(index);
        m_trashCardList.Clear();
    }
}