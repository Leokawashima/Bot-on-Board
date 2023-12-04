using UnityEngine;

namespace Map
{
    public class MapObject : MonoBehaviour
    {
        public MapObject_SO MapObjectSO { get; set; }
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

            return true;
        }

        public void ObjectDestroy(MapManager mapManager_)
        {
            mapManager_.MapObjectList.Remove(this);
            mapManager_.MapState.ReSetMapObject(Position);
            Destroy(gameObject);
        }
    }
}