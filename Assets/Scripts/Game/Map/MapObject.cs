using UnityEngine;

public class MapObject : MonoBehaviour
{
    public MapObject_SO_Template m_SO;
    public Vector2Int m_Pos = Vector2Int.zero;
    public uint NowTurn;
    
    public void Initialize(MapManager mapManager_)
    {
        mapManager_.MapObjectList.Add(this);
        mapManager_.SetObjState(m_Pos, m_SO.m_cost);
        mapManager_.SetCollisionState(m_Pos, m_SO.m_IsCollider);
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
        Destroy(gameObject);
    }
}