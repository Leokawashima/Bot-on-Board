using System;
using UnityEngine;

namespace AI
{
    [Serializable]
    public class AIHealth
    {
        private AIAgent m_operator;

        [field: SerializeField] public HealthState State { get; private set; }
        [field: SerializeField] public float HP { get; private set; }
        [field: SerializeField] public float HPMax { get; private set; }

        public event Action<HealthState> Event_SetState;
        public event Action<AIAgent, float>
            Event_Damage,
            Event_Heal;

        public void Initialize(AIAgent ai_)
        {
            m_operator = ai_;
        }

        public void SetState(HealthState state_)
        {
            State = state_;
            Event_SetState?.Invoke(state_);
        }

        public void Damage(float power_)
        {

        }
        public void Heal(float power_)
        {

        }
    }
}