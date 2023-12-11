using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckCardList : MonoBehaviour
{
    [SerializeField] private CardGenerator m_cardGenerator;

    [SerializeField] private RectTransform m_content;

    [SerializeField] private float m_size = 0.55f;
    [SerializeField] private Vector2 m_position = new(-460, 810);
    [SerializeField] private Vector2 m_offset = new(230, -350);
    [SerializeField] private int m_sheat = 5;

    public static event Action<List<DeckCardDrag>> Event_CardCreated;

    private void Start()
    {
        var _dragCards = new List<DeckCardDrag>();
        for(int i = 0; i < m_cardGenerator.m_mapObjectTable.Data.Length; ++i)
        {
            _dragCards.Add(CardCreate(i));
        }
        Event_CardCreated?.Invoke(_dragCards);
    }

    private DeckCardDrag CardCreate(int index_)
    {
        var _moc = m_cardGenerator.Create(index_, m_content);

        var _rect = _moc.transform as RectTransform;
        _rect.localScale = Vector3.one * m_size;
        _rect.anchoredPosition = m_position + new Vector2(m_offset.x * (index_ % m_sheat), m_offset.y * (int)(index_ / m_sheat));

        var _drag = _moc.gameObject.AddComponent<DeckCardDrag>();
        _drag.Initialize(_moc);

        return _drag;
    }
}