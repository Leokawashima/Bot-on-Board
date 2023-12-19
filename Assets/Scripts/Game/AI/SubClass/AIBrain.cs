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

        public void Initialize(AIAgent ai_)
        {
            m_operator = ai_;
        }

        public void Think(AIAgent ai_)
        {
            // 自身を抜いた敵のリスト 仲間のAIがいる可能性を考慮できていない
            var _enemy = new List<AIAgent>(AIManager.Singleton.AIList);
            _enemy.Remove(ai_);

            var _aStar = new AStarAlgorithm(MapManager.Singleton.Stage);
            // 相手は一人しかいない前提で[0]の座標をターゲットにする
            SearchRoute = _aStar.Search(ai_.Travel.Position, _enemy[0].Travel.Position);
            if (ai_.State.StanTurn > 0)
            {
                --ai_.State.StanTurn;
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