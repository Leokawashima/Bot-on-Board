using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : MonoBehaviour
{
    [field: SerializeField] public int Index { get; private set; } = 0;//仮持たせ　Playerへの参照を取ってPlayerにIndexを持たせる方が都合がいい
    public enum DirectionFrontBack { Non = 0, Forward = 1, Back = -1 }
    public enum DirectionRightLeft { Non = 0, Right = 1, Left = -1 }
    public enum AliveState { Non, Alive, Dead }
    public enum ThinkState { Non, Attack, Move, CantMove, CollisionPredict }

    [field: SerializeField] public DirectionFrontBack AIDirectionVertical { get; private set; } = DirectionFrontBack.Non;
    [field: SerializeField] public DirectionRightLeft AIDirectionHorizontal { get; private set; } = DirectionRightLeft.Non;
    [field: SerializeField] public AliveState AIAliveState { get; private set; } = AliveState.Non;
    [field: SerializeField] public ThinkState AIThinkState { get; private set; } = ThinkState.Non;

    [field: SerializeField] public Vector2Int Position { get; private set; } = Vector2Int.zero;
    [field: SerializeField] public Vector2Int PrePosition { get; private set; } = Vector2Int.zero;
    public Vector2Int Direction { get { return new Vector2Int((int)AIDirectionHorizontal, (int)AIDirectionVertical); } }

    //本実装するなら初期化の値をどこかで決めて引数で設定すべき デバッグ用にシリアライズ
    [SerializeField] float m_HPMax = 10.0f;
    public float m_HP = 0;//ゲームモードがいろいろ完成したらスマブラのように初期値を設定できるようにする
    [SerializeField] float m_Attack = 1.0f;

    [SerializeField] List<Vector2Int> m_Path;

    [field: SerializeField] public int m_PowWeapon { get; set; }
    [field: SerializeField] public int m_UseWeapon { get; set; }
    [field: SerializeField] public int m_Stan { get; set; }

    Vector2Int m_MapSize = Vector2Int.zero;

    //ダメージイベントと回復イベントを別に分けたのは回復エフェクトやダメージエフェクトを別に登録したいから
    public event Action<int, float>  Event_DamageHP;
    public event Action<int, float> Event_HealHP;

    public void Spawn(int index_, string name_, Vector2Int posData_)
    {
        m_HP = m_HPMax;
        AIAliveState = AliveState.Alive;
        Index = index_;
        name = name_;
        Position = posData_;
        PrePosition = posData_;
        transform.position = new Vector3(posData_.x, 0, posData_.y) + MapManager.Singleton.Offset + Vector3.up;

        m_MapSize = new Vector2Int(MapManager.Singleton.Data_SO.y, MapManager.Singleton.Data_SO.x);

        MapManager.Singleton.m_AIManagerList.Add(this);
    }

    public void Think()
    {
        PrePosition = Position;
        
        //自身を抜いた敵のリスト　人数が増えてもこれは基本変わらない
        var enemy = new List<AISystem>(MapManager.Singleton.m_AIManagerList);
        enemy.Remove(this);

        var _aStar = new AStarAlgorithm(m_MapSize, MapManager.Singleton.m_ObjStates);
        m_Path = _aStar.Search(Position, enemy[0].Position);//相手は一人しかいないので必然的に[0]の座標をターゲットにする
        if (m_Stan > 0)
        {
            --m_Stan;
            AIThinkState = ThinkState.CantMove;
        }
        else if (m_Path.Count == 2)//自身の座標から一マス範囲なのでこぶしの射程圏内　なので攻撃志向(超簡易実装)
        {
            AIThinkState = ThinkState.Attack;
        }
        else if (m_Path.Count == 3)
        {
            AIThinkState= ThinkState.CollisionPredict;
        }
        else
        {
            AIThinkState = ThinkState.Move;
        }
    }

    public void Action()
    {
        switch(AIThinkState)
        {
            case ThinkState.Attack:
                // 重みつき確立ランダムを求める　今は9:1なので10％の確率で1が帰る
                if(GetRandomWeightedProbability(9, 1) == 0)// 10%で移動するかも(アホ)
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
                if(GetRandomWeightedProbability(9, 1) == 0)// 10%で攻撃する(アホ)
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
                if(GetRandomWeightedProbability(5, 5) == 0)
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
        m_HP = Mathf.Max(m_HP - damage_, 0.0f);//HP最低値は0,0固定の方がいいやろ...という前提の処理　将来的に変える...？のか？
        if(m_HP == 0.0f)
            AIAliveState = AliveState.Dead;
        Event_DamageHP?.Invoke(Index, m_HP);
    }
    public void HealHP(float heal_)
    {
        m_HP = Mathf.Min(m_HP + heal_, m_HPMax);
        Event_HealHP?.Invoke(Index, m_HP);
    }

    void Move()
    {
        // 壁系の当たり判定オブジェクトならムリ　これも仮実装　コストを持たせて状況に応じて殴らせて移動していくシステムの方がいい
        if(MapManager.Singleton.m_CollisionState[m_Path[1].y, m_Path[1].x]) return;

        // 経路は[0]が現在地点なので[1]が次のチップ
        transform.localPosition = new Vector3(m_Path[1].x, 0, m_Path[1].y) + MapManager.Singleton.Offset + Vector3.up;
        Position = m_Path[1];
        var _dir = Position - PrePosition;
        transform.rotation = Quaternion.LookRotation(new Vector3(_dir.x, 0, _dir.y), Vector3.up);
    }

    void Attack()
    {
        //自身を抜いた敵のリスト　人数が増えてもこれは基本変わらない
        var enemy = new List<AISystem>(MapManager.Singleton.m_AIManagerList);
        enemy.Remove(this);

        //2人前提で[0]に攻撃判定
        var pos = Position - enemy[0].Position;
        var flag = false;
        if (pos.x == 0 && Mathf.Abs(pos.y) == 1) flag = true;//前後一マスずれにいるか否か
        else if (Mathf.Abs(pos.x) == 1 && pos.y == 0) flag = true;//左右一マスズレにいるか否か

        if(flag)
        {
            float _attackPow = m_Attack;
            if (m_UseWeapon > 0)
            {
                _attackPow = m_PowWeapon;
                if(--m_UseWeapon == 0) m_PowWeapon = 0;

            }
            enemy[0].DamageHP(_attackPow);
        }
    }

    int GetRandomWeightedProbability(params int[] weight_)
    {
        int _total = 0;
        foreach(var _value in weight_)
            _total += _value;

        float _random = _total * UnityEngine.Random.value;

        for(int i = 0; i < weight_.Length; ++i)
        {
            // ランダムポイントが重みより小さいなら
            if(_random < weight_[i])
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