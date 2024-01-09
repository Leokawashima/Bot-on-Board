﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Deck.Edit
{
    public class DeckEditCards : MonoBehaviour
    {
        [SerializeField] private MapObjectCard m_prefab;

        [SerializeField] private RectTransform m_content;

        [SerializeField] private float m_size = 0.55f;

        [SerializeField] private InfoCard m_info;

        public event Action<List<CardDrag>> Event_CardCreated;

        public void Initialize()
        {
            m_info.Initialize();

            var _dragCards = new List<CardDrag>();
            for (int i = 0, len = MapTable.Object.Table.Length; i < len; ++i)
            {
                _dragCards.Add(CardCreate(i));
            }
            Event_CardCreated?.Invoke(_dragCards);
        }

        private CardDrag CardCreate(int index_)
        {
            var _moc = Instantiate(m_prefab, m_content);
            _moc.Initialize(index_);

            _moc.Event_Info += (MapObjectCard card_) =>
            {
                m_info.Enable();
                m_info.SetInfo(card_);
            };

            _moc.transform.localScale = Vector2.one * m_size;

            var _drag = _moc.gameObject.AddComponent<CardDrag>();
            _drag.Initialize(_moc);

            return _drag;
        }
    }
}