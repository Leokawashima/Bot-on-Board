using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public MapObject_SO_Template MapObjectSO { get; set; }
    public Vector2Int Position = Vector2Int.zero;
    public uint ElapsedTurn = 0;

    public void Initialize(MapManager mapManager_)
    {
        mapManager_.MapObjectList.Add(this);
        mapManager_.MapState.SetMapObject(Position, MapObjectSO);
    }

    public bool ObjectUpdate(MapManager mapManager_)
    {
        ElapsedTurn++;

        if (MapObjectSO is IDestroy _destroy)
        {
            if (_destroy.TurnMax - ElapsedTurn <= 0)
            {
                ObjectDestroy(mapManager_);
                return false;
            }
        }

        return true;
    }
    public void ObjectDestroy(MapManager mapManager_)
    {
        mapManager_.MapObjectList.Remove(this);
        mapManager_.MapState.ReSetMapObject(Position);
        Destroy(gameObject);
    }
}