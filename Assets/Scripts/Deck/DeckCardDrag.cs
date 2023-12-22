using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// カードがドラッグされたときにイベントを送信するクラス
/// </summary>
public class DeckCardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// カードを保持するフィールド
    /// </summary>
    private MapObjectCard m_card;

    public event Action<Vector2, MapObjectCard> OnBeginDrag;
    public event Action<Vector2> OnDrag, OnEndDrag;

    public void Initialize(MapObjectCard card_)
    {
        m_card = card_;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData_)
    {
        OnBeginDrag?.Invoke(eventData_.position, m_card);
    }

    void IDragHandler.OnDrag(PointerEventData eventData_)
    {
        OnDrag?.Invoke(eventData_.position);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData_)
    {
        OnEndDrag?.Invoke(eventData_.position);
    }
}