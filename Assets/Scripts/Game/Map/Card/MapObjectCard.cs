using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Map;
using Map.Object;

public class MapObjectCard : MonoBehaviour
{
    public MapObject_SO SO { get; private set; } = null;
    public int Index { get; private set; } = -1;

    [field: SerializeField] public CardAppearance Appearance { get; private set; }

    public EventTrigger m_trigger;

    public event Action
        Event_Trash,
        Event_Select;
    public event Action<MapObjectCard> Event_Info;

    public void Initialize(int index_)
    {
        SO = MapTable.Object.Table[index_];
        Index = index_;
        Appearance.Initialize(SO);

        AddEvent(EventTriggerType.PointerClick, OnPointerClick);
    }

    public void Trash()
    {
        Event_Trash?.Invoke();

        Destroy(gameObject);
    }

    public void AddEvent(EventTriggerType type_, UnityAction<BaseEventData> callback_)
    {
        var _entry = new EventTrigger.Entry
        {
            eventID = type_,
        };
        _entry.callback.AddListener(callback_);
        m_trigger.triggers.Add(_entry);
    }

    private void OnPointerClick(BaseEventData eventData_)
    {
        var _event = eventData_ as PointerEventData ;
        if (_event.button == PointerEventData.InputButton.Left)
        {
            Event_Select?.Invoke();
            return;
        }
        if (_event.button == PointerEventData.InputButton.Right)
        {
            Event_Info?.Invoke(this);
            return;
        }
    }
}