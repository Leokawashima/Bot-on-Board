using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    const int
        m_DeckSize = 10,
        m_HandSize = 4,
        m_DrawSize = 2;

    [SerializeField] List<int> m_Deck { get; set; } = new(m_DeckSize) {
        0, 0, 0, 0, 0, 1, 1, 1, 1, 1,//test
    };

    [SerializeField] MapObject_SO_Template[] MO_SO_Array;

    List<int> m_TrashCardList = new();
    List<int> m_HandCardList = new();
    List<int> m_StockCaedList = new();

    [ContextMenu("create")]
    void Initialize()
    {
        var _shuffle = m_Deck;
        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = 0; i < m_HandSize; ++i)
        {
            var _index = Random.Range(0, _shuffle.Count - 1);
            m_HandCardList.Add(_shuffle[_index]);
            
            MO_SO_Array[_shuffle[_index]].CardCreate(transform);

            _shuffle.RemoveAt(_index);
        }
        m_StockCaedList = _shuffle;
    }
}
