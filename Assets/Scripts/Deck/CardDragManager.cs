using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deck
{
    public class CardDragManager : MonoBehaviour
    {
        [SerializeField]
        private DeckEditArea m_deckEditArea;

        [SerializeField]
        private MapObjectCard m_cursorPrefab;
        private MapObjectCard m_cursorCard;

        private RectTransform m_cursorRectTransform;

        public static event Action<MapObjectCard> Event_EndDrag;

        private void OnEnable()
        {
            DeckCardList.Event_CardCreated += OnCardCreated;
        }
        private void OnDisable()
        {
            DeckCardList.Event_CardCreated -= OnCardCreated;
        }

        private void Start()
        {
            m_cursorCard = Instantiate(m_cursorPrefab, transform.parent);
            var _rect = m_cursorCard.transform as RectTransform;
            m_cursorRectTransform = _rect;
            _rect.localScale = (m_cursorPrefab.transform as RectTransform).localScale;
            m_cursorCard.gameObject.SetActive(false);
        }

        private void OnCardCreated(List<CardDrag> drags_)
        {
            foreach (var drag in drags_)
            {
                drag.OnBeginDrag += OnBeginDrag;
                drag.OnDrag += OnDrag;
                drag.OnEndDrag += OnEndDrag;
            }
        }

        private void OnBeginDrag(Vector2 mousePos_, MapObjectCard card_)
        {
            m_cursorCard.gameObject.SetActive(true);
            m_cursorRectTransform.position = mousePos_;

            CopyCard(card_, m_cursorCard);
        }

        private void OnDrag(Vector2 mousePos_)
        {
            if (m_deckEditArea.CheckHitCard(mousePos_, out var _card))
            {
                var _rect = _card.transform as RectTransform;
                m_cursorRectTransform.position = _rect.position + Vector3.up * 90.0f;
                return;
            }

            m_cursorRectTransform.position = mousePos_;
        }

        private void OnEndDrag(Vector2 mousePos_)
        {
            if (m_deckEditArea.CheckHitCard(mousePos_, out var _card))
            {
                Event_EndDrag?.Invoke(m_cursorCard);
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