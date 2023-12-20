using System;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace AI
{
    [Serializable]
    public class AIBrain
    {
        private AIAgent m_operator;

        [field: SerializeField] public ThinkState State { get; private set; }
        [field: SerializeField] public List<Vector2Int> SearchRoute { get; private set; }

        [field: SerializeField] public int Intelligent { get; private set; }
        [SerializeField] public readonly int IntelligentMin = -2;
        [SerializeField] public readonly int IntelligentMax = 2;

        public event Action<AIAgent, int>
            Event_IncreaseIntelligent,
            Event_DecreaseIntelligent;

        public void Initialize(AIAgent ai_)
        {
            m_operator = ai_;
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

        public void Think(AIAgent ai_)
        {
            // 自身を抜いた敵のリスト 仲間のAIがいる可能性を考慮できていない
            var _enemy = new List<AIAgent>(AIManager.Singleton.AIList);
            _enemy.Remove(ai_);

            var _aStar = new AStarAlgorithm(MapManager.Singleton.Stage);
            // 相手は一人しかいない前提で[0]の座標をターゲットにする
            SearchRoute = _aStar.Search(ai_.Travel.Position, _enemy[0].Travel.Position);
            if (ai_.Health.StanTurn > 0)
            {
                --ai_.Health.StanTurn;
                State = ThinkState.CantMove;
            }
            // 自身の座標から一マス範囲なのでこぶしの射程圏内　なので攻撃志向(超簡易実装)
            else if (SearchRoute.Count == 2)
            {
                State = ThinkState.Attack;
            }
            else if (SearchRoute.Count == 3)
            {
                State = ThinkState.CollisionPredict;
            }
            else
            {
                State = ThinkState.Move;
            }
        }
    }
}