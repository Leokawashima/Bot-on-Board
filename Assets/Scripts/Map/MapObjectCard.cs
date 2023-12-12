using System;
using UnityEngine;
using Map;
using Map.Chip;
using Map.Object;

public class MapObjectCard : MonoBehaviour
{
    public MapObject_SO SO;
    public int Index = -1;

    public event Action Event_Trash;

    public void Trash()
    {
        Event_Trash?.Invoke();

        Destroy(gameObject);
    }
}