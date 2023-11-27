using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;
using UnityEngine.UI;

public class DeckCardDragManager : MonoBehaviour
{
    public List<DeckCardDrag> m_cardList { get; private set; } = new();

    [SerializeField]
    private MapObjectCard m_selectionCard;

    [SerializeField]
    private DeckEditArea m_deckEditArea;

    private RectTransform m_selectionRectTransform;

    public static event Action<MapObjectCard> Event_EndDrag;

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
        // セレクトカードに反映
        m_selectionCard.m_SO = card_.m_SO;
        m_selectionCard.m_Index = card_.m_Index;
        m_selectionCard.m_Text.text = card_.m_SO.m_ObjectName;

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
            Event_EndDrag?.Invoke(m_selectionCard);
            // カードに情報を渡す
            _card.m_SO = m_selectionCard.m_SO;
            _card.m_Index = m_selectionCard.m_Index;
            _card.m_Text.text = m_selectionCard.m_SO.m_ObjectName;
        }

        m_selectionCard.gameObject.SetActive(false);

        // 空にする
        m_selectionCard.m_SO = null;
        m_selectionCard.m_Index = -1;
        m_selectionCard.m_Text.text = string.Empty;
    }
}
