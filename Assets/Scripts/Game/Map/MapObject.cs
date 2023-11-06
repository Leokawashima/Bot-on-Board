using UnityEngine;

public class MapObject : MonoBehaviour
{
    public MapObject_SO_Template MapObjectSO { get; set; }
    public Vector2Int Position = Vector2Int.zero;
    public uint NowTurn;

    public void Initialize(MapManager mapManager_)
    {
        mapManager_.MapObjectList.Add(this);
        mapManager_.MapState.SetMapObject(Position, MapObjectSO);
    }

    public bool ObjectUpdate(MapManager mapManager_)
    {
        if (--NowTurn == 0)
        {
            ObjectDestroy(mapManager_);
            return false;
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