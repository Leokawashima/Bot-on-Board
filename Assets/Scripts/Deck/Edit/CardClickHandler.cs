using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    private MapObjectCard m_card;

    public event Action<PointerEventData> Event_Click;

    public void Initialize(MapObjectCard card_)
    {
        m_card = card_;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData_)
    {
        Event_Click?.Invoke(eventData_);
    }
}