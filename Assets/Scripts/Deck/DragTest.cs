using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private MapObjectCard m_Card;

    private readonly Vector2 DisplaySize = new Vector2(1920 / 2.0f, 1080 / 2.0f);

    public event Action<Vector2, MapObjectCard> OnBeginDrag, OnDrag, OnEndDrag;

    private Vector2 GetMousePosition(PointerEventData eventData_)
    {
        return eventData_.position - DisplaySize;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData_)
    {
        OnBeginDrag?.Invoke(GetMousePosition(eventData_), m_Card);
    }

    void IDragHandler.OnDrag(PointerEventData eventData_)
    {
        OnDrag?.Invoke(GetMousePosition(eventData_), m_Card);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData_)
    {
        OnEndDrag?.Invoke(GetMousePosition(eventData_), m_Card);
    }
}