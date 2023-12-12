using System.Collections.Generic;
using UnityEngine;

namespace Map.Chip
{
    public class MapChip : MonoBehaviour
    {
        public MapChip_SO Data { get; set; }
        public Vector2Int Position = Vector2Int.zero;

        public Material Material { get; private set; }

        [field: SerializeReference]
        public List<MapChipComponent> Components { get; private set; } = new();

        public void Initialize(MapManager manager_)
        {
            Material = GetComponent<Renderer>().material;

            foreach (var component in Components)
            {
                component.Initialize();
            }

            manager_.MapState.SetMapChip(Position, this);
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

        public void Ride(AISystem ai_)
        {
            foreach (var component in Components)
            {
                component.Ride(ai_);
            }
        }

        public void Destroy(MapManager manager_)
        {
            foreach (var component in Components)
            {
                component.Destroy();
            }

            manager_.MapState.ResetMapChip(Position);
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}