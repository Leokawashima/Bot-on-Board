using System.Collections.Generic;
using UnityEngine;

public class DeckCardDragManager : MonoBehaviour
{
    public List<DeckCardDrag> m_cardList { get; private set; } = new();

    [SerializeField]
    private MapObjectCard m_selectionCard;

    [SerializeField]
    private DeckEditArea m_deckEditArea;

    private RectTransform m_selectionRectTransform;

    private void Start()
    {
        foreach (var card in m_cardList)
        {
            card.OnBeginDrag += OnBeginDrag;
            card.OnDrag += OnDrag;
            card.OnEndDrag += OnEndDrag;
        }

        m_selectionCard.gameObject.SetActive(false);
        m_selectionRectTransform = m_selectionCard.transform as RectTransform;
    }

    private void OnBeginDrag(Vector2 mousePos_, MapObjectCard card_)
    {
        m_selectionCard.gameObject.SetActive(true);
        m_selectionRectTransform.localPosition = mousePos_;
        //m_selectionCard = card_;
    }

    private void OnDrag(Vector2 mousePos_)
    {
        if (m_deckEditArea.CheckHitCard(mousePos_, out var _card))
        {
            var _rect = _card.transform as RectTransform;
            m_selectionRectTransform.localPosition = _rect.anchoredPosition;
            m_selectionRectTransform.sizeDelta = _rect.sizeDelta;
        }
        else
        {
            m_selectionRectTransform.localPosition = mousePos_;
            m_selectionRectTransform.sizeDelta = new Vector2(100, 100);
        }
    }

    private void OnEndDrag(Vector2 mousePos_)
    {
        if (m_deckEditArea.CheckHitCard(mousePos_, out var _card))
        {
            // カードに情報を渡す
        }

        m_selectionCard.gameObject.SetActive(false);
        //m_selectionCard = null;
    }
}
