using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// カードがドラッグされたときにイベントを送信するクラス
/// </summary>
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private MapObjectCard m_card;

    public event Action<PointerEventData, MapObjectCard> Event_BeginDrag;
    public event Action<PointerEventData> Event_Drag, Event_EndDrag;

    public void Initialize(MapObjectCard card_)
    {
        m_card = card_;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData_)
    {
        Event_BeginDrag?.Invoke(eventData_, m_card);
    }
    void IDragHandler.OnDrag(PointerEventData eventData_)
    {
        Event_Drag?.Invoke(eventData_);
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData_)
    {
        Event_EndDrag?.Invoke(eventData_);
    }
}