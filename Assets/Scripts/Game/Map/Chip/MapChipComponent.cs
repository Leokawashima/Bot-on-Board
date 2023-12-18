using System;
using UnityEngine;
using AI;

namespace Map.Chip.Component
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

        public virtual void Ride(AI.AIAgent ai_)
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

        public override void Ride(AI.AIAgent ai_)
        {
            ai_.Damage(Power);
        }
    }
}