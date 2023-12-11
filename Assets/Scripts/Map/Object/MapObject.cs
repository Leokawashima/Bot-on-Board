using System.Collections.Generic;
using UnityEngine;

namespace Map.Object
{
    public class MapObject : MonoBehaviour
    {
        public MapObject_SO Data { get; set; }
        public Vector2Int Position = Vector2Int.zero;

        [field: SerializeReference]
        public List<MapObjectComponent> Components { get; private set; } = new();

        public void Initialize(MapManager manager_)
        {
            foreach (var component in Components)
            {
                component.Initialize();
            }

            manager_.MapObjectList.Add(this);
            manager_.MapState.SetMapObject(Position, this);
        }

        public bool TurnUpdate(MapManager manager_)
        {
            foreach (var component in Components)
            {
                if (false == component.Update())
                {
                    this.Destroy(manager_);
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

        public void Destroy(MapManager manager_)
        {
            foreach (var component in Components)
            {
                component.Destroy();
            }

            manager_.MapObjectList.Remove(this);
            manager_.MapState.ReSetMapObject(Position);
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}