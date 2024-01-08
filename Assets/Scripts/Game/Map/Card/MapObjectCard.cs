using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Map;
using Map.Object;

public class MapObjectCard : MonoBehaviour, IPointerClickHandler
{
    public MapObject_SO SO { get; private set; } = null;
    public int Index { get; private set; } = -1;

    [field: SerializeField] public CardAppearance Appearance { get; private set; }

    public event Action Event_Trash;
    public event Action<MapObjectCard> Event_Info;

    public void Initialize(int index_)
    {
        SO = MapTable.Object.Table[index_];
        Index = index_;
        Appearance.Initialize(SO);
    }

    public void Trash()
    {
        Event_Trash?.Invoke();

        Destroy(gameObject);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData_)
    {
        if (eventData_.button == PointerEventData.InputButton.Right)
        {
            Event_Info?.Invoke(this);
        }
    }
}