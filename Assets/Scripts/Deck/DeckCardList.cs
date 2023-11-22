using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckCardList : MonoBehaviour
{
    [SerializeField] DeckCardDragManager _deckCardDragManager;
    
    [SerializeField] MapObjectTable_SO m_table;

    [SerializeField] RectTransform m_content;

    private void Start()
    {
        for (int y = 0; y < 2; ++y)
        {
            for (int i = 0; i < m_table.Data.Length; ++i)
            {
                _deckCardDragManager.m_cardList.Add(CardCreate(m_table.Data[i], i, y));
            }
        }
    }

    private DeckCardDrag CardCreate(MapObject_SO_Template mapObject_SO_, int index_, int y_)
    {
        var _moc = Instantiate(mapObject_SO_.m_Card, m_content);
        _moc.m_SO = mapObject_SO_;
        _moc.m_Index = index_;
        _moc.m_Text.text = mapObject_SO_.m_ObjectName;

        var _rect = _moc.transform as RectTransform;
        _rect.anchoredPosition = new Vector2(index_ * 240.0f - 500.0f, 800.0f - 350.0f * y_);

        return _moc.AddComponent<DeckCardDrag>();
    }
}