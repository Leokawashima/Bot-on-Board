using System.Collections.Generic;
using UnityEngine;
using Map.Chip.Component;
using AI;

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

            manager_.Stage.SetMapChip(Position, this);
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

        public void Ride(AI.AIAgent ai_)
        {
            foreach (var component in Components)
            {
                component.Ride(ai_);
            }
        }

        public void Finalize(MapManager manager_)
        {
            foreach (var component in Components)
            {
                component.Destroy();
            }

            manager_.Stage.ResetMapChip(Position);
            Destroy(gameObject);
        }
    }
}