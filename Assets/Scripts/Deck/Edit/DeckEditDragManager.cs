using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Deck.Edit
{
    public class DeckEditDragManager : MonoBehaviour
    {
        private RectTransform m_rectTransform;

        public DeckData EditDeck { get; private set; }
        public List<MapObjectCard> EditCards { get; private set; }

        [SerializeField] private MapObjectCard m_targetPrefab;

        [SerializeField] private MapObjectCard m_cursorCard;
        private RectTransform m_cursorRectTransform;

        public event Action<MapObjectCard, int> Event_EndDrag;

        [SerializeField] private int m_defaultDeckSize = 10;

        public void Initialize()
        {
            DeckEditManager.Singleton.Cards.Event_CardCreated += OnCardCreated;

            m_rectTransform = transform as RectTransform;

            EditDeck = new();
            EditCards = new();

            for (int i = 0, cnt = m_defaultDeckSize; i < cnt; ++i)
            {
                CardCreate();
            }

            m_cursorRectTransform = m_cursorCard.transform as RectTransform;
            m_cursorCard.gameObject.SetActive(false);
        }

        private void CardCreate()
        {
            var _moc = Instantiate(m_targetPrefab, m_rectTransform);
            EditDeck.Cards.Add(_moc.Index);
            EditCards.Add(_moc);
        }

        public bool CheckHitCard(Vector2 mousePos_, out MapObjectCard card_, out int index_)
        {
            if (CheckHit(m_rectTransform, mousePos_))
            {
                for (int i = 0, cnt = EditCards.Count; i < cnt; ++i)
                {
                    var _rect = EditCards[i].transform as RectTransform;
                    if (CheckHit(_rect, mousePos_))
                    {
                        card_ = EditCards[i];
                        index_ = i;
                        return true;
                    }
                }
            }
            card_ = null;
            index_ = -1;
            return false;
        }

        private bool CheckHit(RectTransform rect_, Vector2 mousePos_)
        {
            var _pos = rect_.position;
            var _size = rect_.sizeDelta * rect_.localScale;
            var _sizeHalf = _size / 2.0f;

            return _InSide(_pos.x, _sizeHalf.x, mousePos_.x) && _InSide(_pos.y, _sizeHalf.y, mousePos_.y);

            static bool _InSide(float pos_, float sizeHalf_, float mousePos_)
            {
                return pos_ - sizeHalf_ < mousePos_ && pos_ + sizeHalf_ > mousePos_;
            }
        }

        private void OnCardCreated(List<CardDragHandler> cards_)
        {
            foreach (var card in cards_)
            {
                card.Event_BeginDrag += OnBeginDrag;
                card.Event_Drag += OnDrag;
                card.Event_EndDrag += OnEndDrag;
            }
        }

        private void OnBeginDrag(PointerEventData eventData_, MapObjectCard card_)
        {
            m_cursorCard.gameObject.SetActive(true);
            m_cursorRectTransform.position = eventData_.position;

            CopyCard(card_, m_cursorCard);
        }

        private void OnDrag(PointerEventData eventData_)
        {
            if (CheckHitCard(eventData_.position, out var _card, out var index_))
            {
                var _rect = _card.transform as RectTransform;
                m_cursorRectTransform.position = _rect.position + Vector3.up * 90.0f;
                return;
            }

            m_cursorRectTransform.position = eventData_.position;
            //m_cursorRectTransform.localPosition = eventData_.position;
        }

        private void OnEndDrag(PointerEventData eventData_)
        {
            if (CheckHitCard(eventData_.position, out var _card, out var index_))
            {
                Event_EndDrag?.Invoke(m_cursorCard, index_);
                // カードに情報を渡す
                CopyCard(m_cursorCard, _card);
            }

            m_cursorCard.gameObject.SetActive(false);

            // カーソルカードを空にする
            // 処理を記述するのが面倒なのと書かなくても機能自体はするので書いていない
        }

        private void CopyCard(MapObjectCard from_, MapObjectCard to_)
        {
            to_.Initialize(from_.Index);

            var _fromAppearance = from_.GetComponent<CardAppearance>();
            var _toAppearance = to_.GetComponent<CardAppearance>();

            _toAppearance.Copy(_fromAppearance);
        }
    }
}