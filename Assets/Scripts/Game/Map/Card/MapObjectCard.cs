using System;
using UnityEngine;
using Map;
using Map.Object;

public class MapObjectCard : MonoBehaviour
{
    public MapObject_SO SO { get; private set; }
    public int Index { get; private set; } = -1;

    public event Action Event_Trash;

    public void Initialize(int index_)
    {
        SO = MapTable.Object.Table[index_];
        Index = index_;
    }

    public void Trash()
    {
        Event_Trash?.Invoke();

        Destroy(gameObject);
    }
}