using System;
using UnityEngine;
using Bot;

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

        public virtual void Ride(BotAgent ai_)
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

        public override void Ride(BotAgent ai_)
        {
            ai_.Health.Damage(Power);
        }
    }
}