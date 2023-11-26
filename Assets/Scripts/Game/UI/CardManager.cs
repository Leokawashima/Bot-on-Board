using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    const int
        m_DeckSize = 10,
        m_HandSize = 4,
        m_DrawSize = 2;

    public MapObjectCard GetSelectCard { get
        { return m_ToggleGroup.ActiveToggles().FirstOrDefault()?.GetComponent<MapObjectCard>(); }
    }

    [SerializeField] List<int> m_Deck = new(m_DeckSize) {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9,//デッキを組めるようになったらデッキをJson保存しそれらを呼び出して初期化する
    };

    [SerializeField] MapObjectTable_SO m_MO_SO_Table;
    [SerializeField] ToggleGroup m_ToggleGroup;

    private const float CardSelectOffset = 50.0f;

    public List<int> m_TrashCardList = new();
    public List<int> m_HandCardList = new();
    public List<int> m_StockCaedList = new();

    public void Initialize()
    {
        for (int i = 0; i < m_HandSize; ++i)
        {
            var _index = Random.Range(0, m_Deck.Count - 1);
            m_HandCardList.Add(m_Deck[_index]);

            CardCreate(i, m_MO_SO_Table.Data[m_Deck[i]]);

            m_Deck.RemoveAt(_index);
        }
        m_StockCaedList = new(m_Deck);
        m_Deck.Clear();
    }

    private void CardCreate(int index_, MapObject_SO_Template so_)
    {
        var moc = Instantiate(so_.m_Card, transform);
        moc.m_SO = so_;
        moc.m_Index = index_;
        moc.m_Text.text = so_.m_ObjectName;
        moc.m_Toggle.group = m_ToggleGroup;
        moc.m_CardManager = this;

        var _rect = moc.transform as RectTransform;
        _rect.localScale = Vector2.one * 0.5f;

        moc.m_Toggle.onValueChanged.AddListener((bool isOn_) =>
        {
            _rect.anchoredPosition = new Vector2(
                _rect.anchoredPosition.x,
                isOn_ ?
                _rect.anchoredPosition.y + CardSelectOffset : _rect.anchoredPosition.y - CardSelectOffset
                );
        });
    }

    public void Draw()
    {
        var _space = m_HandSize - m_HandCardList.Count;
        var _draw = Mathf.Min(_space, m_DrawSize);

        for (int i = 0; i < _draw; ++i)
        {
            var _index = Random.Range(0, m_StockCaedList.Count - 1);
            m_HandCardList.Add(m_StockCaedList[_index]);

            CardCreate(i, m_MO_SO_Table.Data[m_StockCaedList[_index]]);

            m_StockCaedList.RemoveAt(_index);
        }

        //ドローが終わったら破棄カードたちを山札に戻す
        foreach(var _index in m_TrashCardList)
            m_StockCaedList.Add(_index);
        m_TrashCardList.Clear();
    }
}