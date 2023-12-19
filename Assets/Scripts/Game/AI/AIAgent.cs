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

        [field: SerializeField] public AIBrain Brain { get; private set; }
        [field: SerializeField] public AITravel Travel { get; private set; }
        [field: SerializeField] public AIState State { get; private set; }

        [field: SerializeField] public Weapon HasWeapon { get; set; }

        [field: SerializeField] public AICamera Camera { get; private set; }

        public AIAgent Spawn(PlayerAgent operator_, Vector2Int pos_)
        {
            name = $"AI：{operator_.Index}";
            Operator = operator_;

            Brain.Initialize(this);
            Travel.Initialize(this, pos_);
            State.Initialize(this);

            transform.localPosition = new Vector3(pos_.x, 1, pos_.y) + MapManager.Singleton.Offset;

            return this;
        }

        public void Think()
        {
            Travel.Clear();
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
                        Travel.Step(Brain.SearchRoute[1]);
                    }
                    break;
                case ThinkState.Move:
                    if (GetRandomWeightedProbability(19, 1) == 0)// 10%で攻撃する(アホ)
                    {
                        Travel.Step(Brain.SearchRoute[1]);
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
                        Travel.Step(Brain.SearchRoute[1]);
                    }
                    else
                    {
                        Attack();
                    }
                    break;
            }
        }

        void Attack()
        {
            // 自身を抜いた敵のリスト
            var _enemy = new List<AIAgent>(AIManager.Singleton.AIList);
            _enemy.Remove(this);

            for (int i = 0; i < _enemy.Count; ++i)
            {
                var _pos = Travel.Position - _enemy[i].Travel.Position;
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