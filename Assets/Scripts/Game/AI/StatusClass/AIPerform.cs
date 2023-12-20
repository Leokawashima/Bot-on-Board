using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [Serializable]
    public class AIPerform
    {
        private AIAgent m_operator;
        [field: SerializeField] public List<Execute> Executes { get; private set; }

        public void Initialize(AIAgent ai_)
        {
            m_operator = ai_;

            Executes = new();
        }

        public void Clear()
        {
            Executes.Clear();
        }

        public void Action()
        {
            if (Executes.Count == 0)
            {
                return;
            }
            var _index = GetRandomWeightedProbability(Executes);
            Executes[_index].Action();
            Clear();
        }

        private int GetRandomWeightedProbability(List<Execute> executes_)
        {
            float _total = 0;
            foreach (var _exe in executes_)
            {
                _total += _exe.Weight;
            }

            float _random = _total * UnityEngine.Random.value;

            for (int i = 0, cnt = executes_.Count; i < cnt; ++i)
            {
                // ランダムポイントが重みより小さいなら
                if (_random < executes_[i].Weight)
                {
                    return i;
                }
                else
                {
                    // ランダムポイントが重みより大きいならその値を引いて次の要素へ
                    _random -= executes_[i].Weight;
                }
            }
            return -1;
        }
    }

    [Serializable]
    public class Execute
    {
        [field: SerializeField] public float Weight { get; private set; }
        [field: SerializeField] public Action Action { get; private set; }

        public Execute(float weight_, Action action_)
        {
            if (weight_ <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(weight_));
            }
            Weight = weight_;
            Action = action_;
        }
    }
}