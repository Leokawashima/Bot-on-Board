using System;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Bot
{
    [Serializable]
    public class BotBrain : BotField_Template
    {
        [field: SerializeField] public ThinkState State { get; private set; }
        [field: SerializeField] public List<Vector2Int> SearchRoute { get; private set; }

        [field: SerializeField] public int Intelligent { get; private set; }
        [SerializeField] private readonly int IntelligentMin = -2;
        [SerializeField] private readonly int IntelligentMax = 2;

        public event Action<BotAgent, int>
            Event_IncreaseIntelligent,
            Event_DecreaseIntelligent;

        public BotBrain(BotAgent bot_) : base(bot_)
        {
            Intelligent = 0;
        }

        public void InceaseIntelligent(uint value_)
        {
            int _increase = (int)value_;
            if (Intelligent >= IntelligentMax)
            {
                return;
            }

            if (Intelligent + _increase > IntelligentMax)
            {
                int _relative = Intelligent + _increase - IntelligentMax;
                Intelligent += _relative;
                Event_IncreaseIntelligent?.Invoke(m_operator, _relative);
            }
            else
            {
                Intelligent += _increase;
                Event_IncreaseIntelligent?.Invoke(m_operator, _increase);
            }
        }
        public void DecreaseIntelligent(uint value_)
        {
            int _decrease = (int)-value_;
            if (Intelligent <= IntelligentMin)
            {
                return;
            }

            if (Intelligent + _decrease < IntelligentMin)
            {
                int _relative = Intelligent + _decrease - IntelligentMin;
                Intelligent += _relative;
                Event_DecreaseIntelligent?.Invoke(m_operator, _relative);
            }
            else
            {
                Intelligent += _decrease;
                Event_DecreaseIntelligent?.Invoke(m_operator, _decrease);
            }
        }

        public void Think(BotAgent ai_)
        {
            // 自身を抜いた敵のリスト 仲間のAIがいる可能性を考慮できていない
            var _enemy = new List<BotAgent>(BotManager.Singleton.Bots);
            _enemy.Remove(ai_);

            var _aStar = new AStarAlgorithm(MapManager.Singleton.Stage);
            // 相手は一人しかいない前提で[0]の座標をターゲットにする
            SearchRoute = _aStar.Search(ai_.Travel.Position, _enemy[0].Travel.Position);
            if (ai_.Health.StanTurn > 0)
            {
                State = ThinkState.CantMove;
                --ai_.Health.StanTurn;
            }
            // 自身の座標から一マス範囲なのでこぶしの射程圏内　なので攻撃志向(超簡易実装)
            else if (SearchRoute.Count == 2)
            {
                State = ThinkState.Attack;
                ai_.Perform.Processes.Add(new(19.0f + Intelligent, () => ai_.Assault.Attack()));
                ai_.Perform.Processes.Add(new(1.0f, () => ai_.Travel.Step(ai_.Brain.SearchRoute[1])));
            }
            else if (SearchRoute.Count == 3)
            {
                State = ThinkState.CollisionPredict;
                ai_.Perform.Processes.Add(new(5.0f, () => ai_.Travel.Step(ai_.Brain.SearchRoute[1])));
                ai_.Perform.Processes.Add(new(5.0f, () => ai_.Assault.Attack()));
            }
            else
            {
                State = ThinkState.Move;
                ai_.Perform.Processes.Add(new(19.0f + Intelligent, () => ai_.Travel.Step(ai_.Brain.SearchRoute[1])));
                ai_.Perform.Processes.Add(new(1.0f, () => ai_.Assault.Attack()));
            }
        }
    }
}