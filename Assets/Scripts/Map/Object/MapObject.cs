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

        public T GetMOComponent<T>() where T : MapObjectComponent
        {
            foreach(var component in Components)
            {
                if (component.GetType() == typeof(T))
                {
                    return component as T;
                }
            }
            return null;
        }

        public void Initialize(MapManager manager_)
        {
            foreach (var component in Components)
            {
                component.Initialize(this);
            }

            manager_.MapObjectList.Add(this);
            manager_.MapState.SetMapObject(Position, this);
        }

        public bool TurnUpdate(MapManager manager_)
        {
            foreach (var component in Components)
            {
                if (false == component.Update(this))
                {
                    Finalize(manager_);
                    return false;
                }
            }

            return true;
        }

        public void Hit(AISystem ai_)
        {
            foreach (var component in Components)
            {
                component.Hit(this, ai_);
            }
        }

        public void Finalize(MapManager manager_)
        {
            foreach (var component in Components)
            {
                component.Destroy(this);
            }

            manager_.MapObjectList.Remove(this);
            manager_.MapState.ReSetMapObject(Position);
            Destroy(gameObject);
        }
    }
}