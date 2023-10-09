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
        0, 0, 1, 1, 2, 2, 3, 3, 4, 4,//デッキを組めるようになったらデッキをJson保存しそれらを呼び出して初期化する
    };

    [SerializeField] MapObject_SO_Template[] MO_SO_Array;
    [SerializeField] ToggleGroup m_ToggleGroup;

    public List<int> m_TrashCardList = new();
    public List<int> m_HandCardList = new();
    public List<int> m_StockCaedList = new();

    public void Initialize()
    {
        for (int i = 0; i < m_HandSize; ++i)
        {
            var _index = Random.Range(0, m_Deck.Count - 1);
            m_HandCardList.Add(m_Deck[_index]);
            
            MO_SO_Array[m_Deck[_index]].CardCreate(m_Deck[_index], transform, m_ToggleGroup, this);

            m_Deck.RemoveAt(_index);
        }
        m_StockCaedList = new(m_Deck);
        m_Deck.Clear();
    }

    public void Draw()
    {
        var _space = m_HandSize - m_HandCardList.Count;
        var _draw = Mathf.Min(_space, m_DrawSize);

        for (int i = 0; i < _draw; ++i)
        {
            var _index = Random.Range(0, m_StockCaedList.Count - 1);
            m_HandCardList.Add(m_StockCaedList[_index]);

            MO_SO_Array[m_StockCaedList[_index]].CardCreate(m_StockCaedList[_index], transform, m_ToggleGroup, this);

            m_StockCaedList.RemoveAt(_index);
        }

        //ドローが終わったら破棄カードたちを山札に戻す
        foreach(var _index in m_TrashCardList)
            m_StockCaedList.Add(_index);
        m_TrashCardList.Clear();
    }
}
