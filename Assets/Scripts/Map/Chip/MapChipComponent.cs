using System;
using UnityEngine;

namespace Map.Chip
{
    [Serializable]
    public abstract class MapChipComponent
    {
        public virtual void Initialize()
        {
        }

        public virtual bool Update()
        {
            return true;
        }

        public virtual void Ride(AISystem ai_)
        {
        }

        public virtual void Destroy()
        {
        }
    }

    public class Damage : MapChipComponent
    {
        [Header(nameof(Damage))]
        [SerializeField] private float Power = 1.0f;

        public override void Ride(AISystem ai_)
        {
            ai_.DamageHP(Power);
        }
    }
}