using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckCardDragManager : MonoBehaviour
{
    [SerializeField]
    private DeckEditArea m_deckEditArea;

    [SerializeField]
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
        m_cursorCard.gameObject.SetActive(false);
        m_cursorRectTransform = m_cursorCard.transform as RectTransform;
    }

    private void OnCardCreated(List<DeckCardDrag> drags_)
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
        m_cursorRectTransform.localPosition = mousePos_;

        // カーソルカードに反映
        m_cursorCard.m_SO = card_.m_SO;
        m_cursorCard.m_Index = card_.m_Index;
        m_cursorCard.m_Text.text = card_.m_SO.m_ObjectName;
    }

    private void OnDrag(Vector2 mousePos_)
    {
        if (m_deckEditArea.CheckHitCard(mousePos_, out var _card))
        {
            var _rect = _card.transform as RectTransform;
            m_cursorRectTransform.localPosition = _rect.anchoredPosition;
            m_cursorRectTransform.sizeDelta = _rect.sizeDelta;
        }
        else
        {
            m_cursorRectTransform.localPosition = mousePos_;
            m_cursorRectTransform.sizeDelta = new Vector2(100, 100);
        }
    }

    private void OnEndDrag(Vector2 mousePos_)
    {
        if (m_deckEditArea.CheckHitCard(mousePos_, out var _card))
        {
            Event_EndDrag?.Invoke(m_cursorCard);
            // カードに情報を渡す
            _card.m_SO = m_cursorCard.m_SO;
            _card.m_Index = m_cursorCard.m_Index;
            _card.m_Text.text = m_cursorCard.m_SO.m_ObjectName;
        }

        m_cursorCard.gameObject.SetActive(false);

        // カーソルカードを空にする
        m_cursorCard.m_SO = null;
        m_cursorCard.m_Index = -1;
        m_cursorCard.m_Text.text = string.Empty;
    }
}
