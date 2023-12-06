using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapObject : MonoBehaviour
    {
        public MapObject_SO Data { get; set; }
        public Vector2Int Position = Vector2Int.zero;

        [field: SerializeReference]
        public List<MOComponent> Components { get; private set; } = new();

        public void Initialize(MapManager mapManager_)
        {
            foreach (var component in Components)
            {
                component.Initialize();
            }

            mapManager_.MapObjectList.Add(this);
            mapManager_.MapState.SetMapObject(Position, this);
        }

        public bool TurnUpdate(MapManager mapManager_)
        {
            foreach (var component in Components)
            {
                if (false == component.Update())
                {
                    Destroy(mapManager_);
                    return false;
                }
            }

            return true;
        }

        public void Hit(AISystem ai_)
        {
            foreach (var component in Components)
            {
                component.Hit(ai_);
            }
        }

        public void Destroy(MapManager mapManager_)
        {
            foreach (var component in Components)
            {
                component.Destroy();
            }

            mapManager_.MapObjectList.Remove(this);
            mapManager_.MapState.ReSetMapObject(Position);
            Destroy(gameObject);
        }
    }
}