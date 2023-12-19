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

        [field: SerializeField] public float HP { get; private set; }
        [field: SerializeField] public float HPMax { get; private set; }
        [field: SerializeField] public float AttackPower { get; private set; }

        [field: SerializeField] public Map.Direction Direction { get; private set; }
        [field: SerializeField] public AliveState AIAliveState { get; private set; }
        [field: SerializeField] public ThinkState AIThinkState { get; private set; }

        [field: SerializeField] public List<Vector2Int> SearchPath { get; private set; }
        [field: SerializeField] public AIPath Move { get; private set; }

        [field: SerializeField] public Weapon HasWeapon { get; set; }
        [field: SerializeField] public uint StanTurn { get; set; }

        public event Action<AIAgent, float>
            Event_DamageHP,
            Event_HealHP;

        [field: SerializeField] public AICamera Camera { get; private set; }

        public AIAgent Spawn(PlayerAgent operator_, Vector2Int posData_)
        {
            Operator = operator_;

            HPMax = 10.0f; // 仮初期設定
            HP = HPMax;
            AttackPower = 1.0f; // 仮初期設定

            AIAliveState = AliveState.Alive;

            Position = posData_;
            PrePosition = posData_;

            transform.localPosition = new Vector3(posData_.x, 1, posData_.y) + MapManager.Singleton.Offset;

            return this;
        }

        public void Think()
        {
            Move.Initialize();

            //自身を抜いた敵のリスト　人数が増えてもこれは基本変わらない
            var enemy = new List<AIAgent>(MapManager.Singleton.AIManagerList);
            enemy.Remove(this);

            var _aStar = new AStarAlgorithm(MapManager.Singleton.Stage);
            SearchPath = _aStar.Search(Position, enemy[0].Position);//相手は一人しかいないので必然的に[0]の座標をターゲットにする
            if (StanTurn > 0)
            {
                --StanTurn;
                AIThinkState = ThinkState.CantMove;
            }
            else if (SearchPath.Count == 2)//自身の座標から一マス範囲なのでこぶしの射程圏内　なので攻撃志向(超簡易実装)
            {
                AIThinkState = ThinkState.Attack;
            }
            else if (SearchPath.Count == 3)
            {
                AIThinkState = ThinkState.CollisionPredict;
            }
            else
            {
                AIThinkState = ThinkState.Move;
            }
        }

        public void Action()
        {
            switch (AIThinkState)
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
                        Move.Step(SearchPath[1]);
                    }
                    break;
                case ThinkState.Move:
                    if (GetRandomWeightedProbability(19, 1) == 0)// 10%で攻撃する(アホ)
                    {
                        Move.Step(SearchPath[1]);
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
                        Move.Step(SearchPath[1]);
                    }
                    else
                    {
                        Attack();
                    }
                    break;
            }
        }

        public void Damage(float damage_)
        {
            //HP最低値は0,0固定の方がいいやろ...という前提の処理　将来的に蘇生される可能性も考慮
            HP = Mathf.Max(HP - damage_, 0.0f);
            if (HP == 0.0f)
            {
                AIAliveState = AliveState.Dead;
            }
            Event_DamageHP?.Invoke(this, damage_);
        }
        public void Heal(float heal_)
        {
            HP = Mathf.Min(HP + heal_, HPMax);
            Event_HealHP?.Invoke(this, heal_);
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
                    PrePosition = Move.Path[i - 1].Position;
                }
                Position = Move.Path[i].Position;
                switch (Move.Path[i].State)
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
            //自身を抜いた敵のリスト
            var _enemy = new List<AIAgent>(MapManager.Singleton.AIManagerList);
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
                        _enemy[0].Damage(AttackPower);
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