using System;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class DeckCardList : MonoBehaviour
{
    [SerializeField] private CardGenerator m_cardGenerator;

    [SerializeField] private RectTransform m_content;

    [SerializeField] private float m_size = 0.55f;

    public static event Action<List<DeckCardDrag>> Event_CardCreated;

    private void Start()
    {
        var _dragCards = new List<DeckCardDrag>();
        for(int i = 0, len = MapTable.Object.Table.Length; i < len; ++i)
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

        var _drag = _moc.gameObject.AddComponent<DeckCardDrag>();
        _drag.Initialize(_moc);

        return _drag;
    }
}