using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bot
{
    [Serializable]
    public class BotPerform : BotField_Template
    {
        [field: SerializeField] public List<WeightProcess> Processes { get; private set; }

        public BotPerform(BotAgent bot_) : base(bot_)
        {
            Processes = new();
        }

        public void Clear()
        {
            Processes.Clear();
        }

        public void Action()
        {
            if (Processes.Count == 0)
            {
                return;
            }
            var _index = GetRandomWeightedProbability(Processes);
            Processes[_index].Action();
            Clear();
        }

        private int GetRandomWeightedProbability(List<WeightProcess> processes_)
        {
            float _total = 0;
            foreach (var process in processes_)
            {
                _total += process.Weight;
            }

            float _random = _total * UnityEngine.Random.value;

            for (int i = 0, cnt = processes_.Count; i < cnt; ++i)
            {
                // ランダムポイントがその重み内なら返す
                if (_random < processes_[i].Weight)
                {
                    return i;
                }
                // 値を引いて次の要素へ
                _random -= processes_[i].Weight;
            }
            return -1;
        }
    }

    [Serializable]
    public class WeightProcess
    {
        [field: SerializeField] public float Weight { get; private set; }
        [field: SerializeField] public Action Action { get; private set; }

        public WeightProcess(float weight_, Action action_)
        {
            // 重みが-1のような値だったら例外を投げる
#if UNITY_EDITOR
            if (weight_ <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(weight_));
            }
#endif
            Weight = weight_;
            Action = action_;
        }
    }
}