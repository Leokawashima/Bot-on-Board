using System;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Deck
{
    public class DeckCardList : MonoBehaviour
    {
        [SerializeField] private CardGenerator m_cardGenerator;

        [SerializeField] private RectTransform m_content;

        [SerializeField] private float m_size = 0.55f;

        [SerializeField] private InfoCard m_info;

        public static event Action<List<CardDrag>> Event_CardCreated;

        private void Start()
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
            var _moc = m_cardGenerator.Create(index_, m_content);

            _moc.Event_Info += (MapObjectCard card_) =>
            {
                m_info.Enable();
                m_info.SetInfo(card_);
            };

            var _rect = _moc.transform as RectTransform;
            _rect.localScale = Vector3.one * m_size;

            var _drag = _moc.gameObject.AddComponent<CardDrag>();
            _drag.Initialize(_moc);

            return _drag;
        }
    }
}