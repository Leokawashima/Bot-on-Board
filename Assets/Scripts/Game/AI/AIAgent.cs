using System;
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
        // このAIを保持しているPlayer
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
        [field: SerializeField] public List<Vector2Int> MovePath { get; private set; }

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
            PrePosition = Position;

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
                    if (GetRandomWeightedProbability(9, 1) == 0)// 10%で移動するかも(アホ)
                    {
                        // 仕様書の賢さレベルに応じて賢さ+2なら100%, +1なら95%, +-0なら90%, -1なら85%, -2なら80%
                        // で不定の動き...以下の処理で言うMoveが呼び出される...という感じになる
                        Attack();
                    }
                    else
                    {
                        Move();
                    }
                    break;
                case ThinkState.Move:
                    if (GetRandomWeightedProbability(9, 1) == 0)// 10%で攻撃する(アホ)
                    {
                        Move();
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
                        Move();
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
        public void DamageHP(float damage_)
        {
            HP = Mathf.Max(HP - damage_, 0.0f);//HP最低値は0,0固定の方がいいやろ...という前提の処理　将来的に変える...？のか？
            if (HP == 0.0f)
                AIAliveState = AliveState.Dead;
            Event_DamageHP?.Invoke(this, damage_);
        }
        public void HealHP(float heal_)
        {
            HP = Mathf.Min(HP + heal_, HPMax);
            Event_HealHP?.Invoke(this, heal_);
        }

        public void Move()
        {
            // 壁系の当たり判定オブジェクトならムリ　これも仮実装　コストを持たせて状況に応じて殴らせて移動していくシステムに変更する
            if (MapManager.Singleton.Stage.Object[SearchPath[1].y][SearchPath[1].x] != null)
            {
                if (MapManager.Singleton.Stage.Object[SearchPath[1].y][SearchPath[1].x].Data.IsCollider)
                    return;
            }

            // 経路は[0]が現在地点なので[1]が次のチップ
            transform.localPosition = new Vector3(SearchPath[1].x, 0, SearchPath[1].y) + MapManager.Singleton.Offset + Vector3.up;
            Position = SearchPath[1];
            var _dir = Position - PrePosition;
            transform.rotation = Quaternion.LookRotation(new Vector3(_dir.x, 0, _dir.y), Vector3.up);
        }

        public void Move(Vector2Int pos_)
        {
            // 壁系の当たり判定オブジェクトならムリ　これも仮実装　コストを持たせて状況に応じて殴らせて移動していくシステムに変更する
            if (MapManager.Singleton.Stage.Object[pos_.y][pos_.x] != null)
            {
                if (MapManager.Singleton.Stage.Object[pos_.y][pos_.x].Data.IsCollider)
                    return;
            }
            // 経路は[0]が現在地点なので[1]が次のチップ
            transform.localPosition = new Vector3(pos_.x, 0, pos_.y) + MapManager.Singleton.Offset + Vector3.up;
            Position = pos_;
            var _dir = Position - PrePosition;
            transform.rotation = Quaternion.LookRotation(new Vector3(_dir.x, 0, _dir.y), Vector3.up);
        }

        #region Attack

        void Attack()
        {
            //自身を抜いた敵のリスト　人数が増えてもこれは基本変わらない
            var enemy = new List<AIAgent>(MapManager.Singleton.AIManagerList);
            enemy.Remove(this);

            //2人前提で[0]に攻撃判定
            var pos = Position - enemy[0].Position;
            var flag = false;
            if (pos.x == 0 && Mathf.Abs(pos.y) == 1) flag = true;//前後一マスずれにいるか否か
            else if (Mathf.Abs(pos.x) == 1 && pos.y == 0) flag = true;//左右一マスズレにいるか否か

            if (flag)
            {
                if (HasWeapon != null)
                {
                    if (HasWeapon.CheckCollider())
                    {
                        if (false == HasWeapon.Action(this))
                        {
                            HasWeapon = null;
                        }
                    }
                    else
                    {
                        enemy[0].DamageHP(AttackPower);
                    }
                }
                else
                {
                    enemy[0].DamageHP(AttackPower);
                }
            }
        }

        [ContextMenu("DebugAttack")]
        void DebugAttack()
        {
            //自身を抜いた敵のリスト　人数が増えてもこれは基本変わらない
            var enemy = new List<AIAgent>(MapManager.Singleton.AIManagerList);
            enemy.Remove(this);

            if (HasWeapon != null)
            {
                if (false == HasWeapon.Action(this))
                {
                    HasWeapon = null;
                }
            }
            else
            {
                enemy[0].DamageHP(AttackPower);
            }
        }

        #endregion Attack

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