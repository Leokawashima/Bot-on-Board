using System.Collections.Generic;
using UnityEngine;
using Map.Object.Component;
using Bot;

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

            manager_.MapObjects.Add(this);
            manager_.Stage.SetMapObject(Position, this);
        }

        public bool TurnUpdate(MapManager manager_)
        {
            foreach (var component in Components)
            {
                if (false == component.Update())
                {
                    Finalize(manager_);
                    return false;
                }
            }

            return true;
        }

        public void Hit(BotAgent ai_)
        {
            foreach (var component in Components)
            {
                component.Hit(ai_);
            }
        }

        public void Finalize(MapManager manager_)
        {
            foreach (var component in Components)
            {
                component.Destroy();
            }

            manager_.MapObjects.Remove(this);
            manager_.Stage.ReSetMapObject(Position);
            Destroy(gameObject);
        }
    }
}