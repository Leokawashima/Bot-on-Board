using Map.Object.Component;
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

        [field: SerializeField] public uint StanTurn { get; set; }

        public event Action<HealthState> Event_SetState;
        public event Action<AIAgent, float>
            Event_Damage,
            Event_Heal;

        public void Initialize(AIAgent ai_)
        {
            m_operator = ai_;

            State = HealthState.Alive;

            HPMax = 10.0f; // 仮初期設定
            HP = HPMax;

            StanTurn = 0;
        }

        public void SetState(HealthState state_)
        {
            State = state_;
            Event_SetState?.Invoke(state_);
        }

        public void Damage(float power_)
        {
            // HP最低値は0,0固定の方がいいやろ...という前提の処理
            // 将来的にマイナスまでいくことや蘇生される可能性も考慮すべき
            HP = Mathf.Max(HP - power_, 0.0f);
            if (HP == 0.0f)
            {
                State = HealthState.Dead;
            }
            Event_Damage?.Invoke(m_operator, power_);
        }
        public void Heal(float power_)
        {
            if (HP >= HPMax)
            {
                return;
            }

            if (HP + power_ > HPMax)
            {
                var _realPower = HP + power_ - HPMax;
                HP += _realPower;
                Event_Heal?.Invoke(m_operator, _realPower);
            }
            else
            {
                HP += power_;
                Event_Heal?.Invoke(m_operator, power_);
            }
        }
    }
}