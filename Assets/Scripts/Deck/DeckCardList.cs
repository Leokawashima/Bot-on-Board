using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckCardList : MonoBehaviour
{
    [SerializeField] private MapObjectTable_SO m_table;//どこからでもアクセスできるテーブルクラスを作るべき

    [SerializeField] private RectTransform m_content;

    [SerializeField] private float m_size = 0.55f;
    [SerializeField] private Vector2 m_position = new(-460, 810);
    [SerializeField] private Vector2 m_offset = new(230, -350);
    [SerializeField] private int m_sheat = 5;

    public static event Action<List<DeckCardDrag>> Event_CardCreated;

    private void Start()
    {
        var _dragCards = new List<DeckCardDrag>();
        for(int i = 0; i < m_table.Data.Length; ++i)
        {
            _dragCards.Add(CardCreate(i));
        }
        Event_CardCreated?.Invoke(_dragCards);
    }

    private DeckCardDrag CardCreate(int index_)
    {
        var _so = m_table.Data[index_];
        var _moc = Instantiate(_so.m_Card, m_content);

        _moc.m_SO = _so;
        _moc.m_Index = index_;
        _moc.m_Text.text = _so.m_ObjectName;

        var _rect = _moc.transform as RectTransform;
        _rect.localScale = Vector3.one * m_size;
        _rect.anchoredPosition = m_position + new Vector2(m_offset.x * (index_ % m_sheat), m_offset.y * (int)(index_ / m_sheat));

        var _drag = _moc.gameObject.AddComponent<DeckCardDrag>();
        _drag.Initialize(_moc);

        return _drag;
    }
}