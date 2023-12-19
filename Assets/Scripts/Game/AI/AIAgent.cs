using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Map.Object.Component;
using Player;

/// <summary>
/// AI単体を処理するクラス
/// </summary>
namespace AI
{
    public class AIAgent : MonoBehaviour
    {
        /// <summary>
        /// このAIを保持しているPlayer
        /// </summary>
        public PlayerAgent Operator { get; private set; }

        [field: SerializeField] public Vector2Int Position { get; private set; }
        [field: SerializeField] public Vector2Int PrePosition { get; private set; }

        [field: SerializeField] public AIBrain Brain { get; private set; }
        [field: SerializeField] public AIMove Move { get; private set; }
        [field: SerializeField] public AIState State { get; private set; }

        [field: SerializeField] public Weapon HasWeapon { get; set; }

        [field: SerializeField] public AICamera Camera { get; private set; }

        public AIAgent Spawn(PlayerAgent operator_, Vector2Int posData_)
        {
            Operator = operator_;

            Position = posData_;
            PrePosition = posData_;

            Brain.Initialize(this);
            Move.Initialize(this);
            State.Initialize(this);

            transform.localPosition = new Vector3(posData_.x, 1, posData_.y) + MapManager.Singleton.Offset;

            return this;
        }

        public void Think()
        {
            Move.Initialize(this);

            Brain.Think(this);
        }

        public void Action()
        {
            switch (Brain.State)
            {
                case ThinkState.Attack:
                    // 重みつき確立ランダムを求める　今は9:1なので10％の確率で1が帰る
                    if (GetRandomWeightedProbability(19, 1) == 0)// 10%で移動するかも(アホ)
                    {
                        // 仕様書の賢さレベルに応じて賢さ+2なら100%, +1なら95%, +-0なら90%, -1なら85%, -2なら80%
                        // で不定の動き...以下の処理で言うMoveが呼び出される...という感じになる
                        Attack();
                    }
                    else
                    {
                        Move.Step(Brain.SearchRoute[1]);
                    }
                    break;
                case ThinkState.Move:
                    if (GetRandomWeightedProbability(19, 1) == 0)// 10%で攻撃する(アホ)
                    {
                        Move.Step(Brain.SearchRoute[1]);
                    }
                    else
                    {
                        Attack();
                    }
                    break;
                case ThinkState.CantMove:
                    break;
                case ThinkState.CollisionPredict:
                    // 移動した場合相手に当たる可能性がある(Pathのカウント3)の場合50/50で移動か攻撃をする
                    if (GetRandomWeightedProbability(5, 5) == 0)
                    {
                        Move.Step(Brain.SearchRoute[1]);
                    }
                    else
                    {
                        Attack();
                    }
                    break;
            }
        }

        
        public void BackPosition()
        {
            Position = PrePosition;
            transform.localPosition = new Vector3(PrePosition.x, 0, PrePosition.y) + MapManager.Singleton.Offset + Vector3.up;
        }

        public IEnumerator DelayMove()
        {
            for (int i = 0, cnt = Move.Count; i < cnt; ++i)
            {
                if (i != 0)
                {
                    PrePosition = Move.Route[i - 1].Position;
                }
                Position = Move.Route[i].Position;
                switch (Move.Route[i].State)
                {
                    case MoveState.Step:
                        for (int j = 1; j <= 10; ++j)
                        {
                            Vector2 prepos = PrePosition;
                            Vector2 pos = Position;
                            Vector2 _offset = (pos - prepos) * j / 10.0f;
                            transform.localPosition = new Vector3(prepos.x, 1, prepos.y)
                                + new Vector3(_offset.x, 0, _offset.y) + MapManager.Singleton.Offset;
                            var _dir = Position - PrePosition;
                            transform.rotation = Quaternion.LookRotation(new Vector3(_dir.x, 0, _dir.y), Vector3.up);
                            yield return new WaitForSeconds(0.1f);
                        }
                        break;
                    case MoveState.Warp:
                        {
                            transform.localPosition = new Vector3(Position.x, 1, Position.y) + MapManager.Singleton.Offset;
                            yield return new WaitForSeconds(1.0f);
                        }
                        break;
                }
                
            }
            PrePosition = Position;
            Move.Clear();
        }

        void Attack()
        {
            // 自身を抜いた敵のリスト
            var _enemy = new List<AIAgent>(AIManager.Singleton.AIList);
            _enemy.Remove(this);

            for (int i = 0; i < _enemy.Count; ++i)
            {
                var _pos = Position - _enemy[i].Position;
                // マンハッタン距離法　前後左右一マスかチェック
                if (1 == (Mathf.Abs(_pos.x) + Mathf.Abs(_pos.y)))
                {
                    if (HasWeapon != null && HasWeapon.CheckCollider())
                    {
                        if (false == HasWeapon.Action(this))
                        {
                            HasWeapon = null;
                        }
                    }
                    else
                    {
                        _enemy[0].State.Damage(State.AttackPower);
                    }
                    break;
                }
            }
        }

        int GetRandomWeightedProbability(params int[] weight_)
        {
            int _total = 0;
            foreach (var _value in weight_)
                _total += _value;

            float _random = _total * UnityEngine.Random.value;

            for (int i = 0; i < weight_.Length; ++i)
            {
                // ランダムポイントが重みより小さいなら
                if (_random < weight_[i])
                {
                    return i;
                }
                else
                {
                    // ランダムポイントが重みより大きいならその値を引いて次の要素へ
                    _random -= weight_[i];
                }
            }
            return default;
        }
    }
}