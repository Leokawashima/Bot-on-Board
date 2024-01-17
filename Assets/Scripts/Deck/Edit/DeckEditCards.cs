using System;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

namespace Deck.Edit
{
    public class DeckEditCards : MonoBehaviour
    {
        [SerializeField] private MapObjectCard m_prefab;

        [SerializeField] private RectTransform m_content;

        [SerializeField] private float m_size = 0.55f;

        [SerializeField] private InfoCard m_info;

        public event Action<List<CardDragHandler>> Event_CardCreated;

        public void Initialize()
        {
            m_info.Initialize();

            var _cards = new List<CardDragHandler>();
            for (int i = 0, len = MapTable.Object.Table.Length; i < len; ++i)
            {
                _cards.Add(CardCreate(i));
            }
            Event_CardCreated?.Invoke(_cards);
        }

        private CardDragHandler CardCreate(int index_)
        {
            var _moc = Instantiate(m_prefab, m_content);
            _moc.Initialize(index_);

            _moc.transform.localScale = Vector2.one * m_size;

            var _click = _moc.AddComponent<CardClickHandler>();
            _click.Initialize(_moc);

            _click.Event_Click += OnClick;
            void OnClick(PointerEventData eventData_)
            {
                if (eventData_.button == PointerEventData.InputButton.Right)
                {
                    m_info.Enable();
                    m_info.SetInfo(_moc);
                }
            }

            var _drag = _moc.AddComponent<CardDragHandler>();
            _drag.Initialize(_moc);

            return _drag;
        }
    }
}