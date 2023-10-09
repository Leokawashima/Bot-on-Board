using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : MonoBehaviour
{
    public int m_Index { get; private set; } = 0;//仮持たせ　Playerへの参照を取ってPlayerにIndexを持たせる方が都合がいい
    public enum Dir_FB { Non = 0, Forward = 1, Back = -1 }
    public enum Dir_RL { Non = 0, Right = 1, Left = -1 }
    public enum State { Non, Alive, Dead }
    public enum ThinkState { Non, Attack, Move, CollisionPredict }

    public Dir_FB m_Dir_FB { get; private set; } = Dir_FB.Non;
    public Dir_RL m_Dir_RL { get; private set; } = Dir_RL.Non;
    public State m_State { get; private set; } = State.Non;
    public ThinkState m_ThinkState { get; private set; } = ThinkState.Non;

    public Vector2Int m_Position { get; private set; } = Vector2Int.zero;
    public Vector2Int m_PrePosition { get; private set; } = Vector2Int.zero;
    public Vector2Int m_Direction { get { return new Vector2Int((int)m_Dir_RL, (int)m_Dir_FB); } }

    //本実装するなら初期化の値をどこかで決めて引数で設定すべき デバッグ用にシリアライズ
    [SerializeField] float m_HPMax = 10.0f;
    public float m_HP = 0;//ゲームモードがいろいろ完成したらスマブラのように初期値を設定できるようにする
    [SerializeField] float m_Attack = 1.0f;

    [SerializeField] List<Vector2Int> m_Path;

    Vector2Int m_MapSize = Vector2Int.zero;

    //ダメージイベントと回復イベントを別に分けたのは回復エフェクトやダメージエフェクトを別に登録したいから
    public event Action<int, float>  Event_DamageHP;
    public event Action<int, float> Event_HealHP;

    public void Spawn(int index_, string name_, Vector2Int posData_)
    {
        m_HP = m_HPMax;
        m_State = State.Alive;
        m_Index = index_;
        name = name_;
        m_Position = posData_;
        m_PrePosition = posData_;
        transform.position = new Vector3(posData_.x, 0, posData_.y) + MapManager.Singleton.Offset + Vector3.up;

        m_MapSize = new Vector2Int(MapManager.Singleton.Data_SO.y, MapManager.Singleton.Data_SO.x);

        MapManager.Singleton.m_AIManagerList.Add(this);
    }

    public void Think()
    {
        m_PrePosition = m_Position;
        
        //自身を抜いた敵のリスト　人数が増えてもこれは基本変わらない
        var enemy = new List<AISystem>(MapManager.Singleton.m_AIManagerList);
        enemy.Remove(this);

        var _aStar = new AStarAlgorithm(m_MapSize, MapManager.Singleton.m_ObjStates);
        m_Path = _aStar.Search(m_Position, enemy[0].m_Position);//相手は一人しかいないので必然的に[0]の座標をターゲットにする

        if (m_Path.Count == 2)//自身の座標から一マス範囲なのでこぶしの射程圏内　なので攻撃志向(超簡易実装)
        {
            m_ThinkState = ThinkState.Attack;
        }
        else if (m_Path.Count == 3)
        {
            m_ThinkState= ThinkState.CollisionPredict;
        }
        else
        {
            m_ThinkState = ThinkState.Move;
        }
    }

    public void Action()
    {
        var _random = UnityEngine.Random.Range(0, 10);
        switch(m_ThinkState)
        {
            case ThinkState.Attack:
                if(_random != 0)//10%で移動するかも(アホ)
                {
                    //仕様書の賢さレベルに応じて賢さ+2なら100%, +1なら95%, +-0なら90%, -1なら85%, -2なら80%
                    //で不定の動き...以下の処理で言うMoveが呼び出される...という感じになる
                    Attack();
                }
                else
                {
                    Move();
                }
                break;
            case ThinkState.Move:
                if(_random != 0)//10%で攻撃する(アホ)
                {
                    Move();
                }
                else
                {
                    Attack();
                }
                break;
            case ThinkState.CollisionPredict:
                if((int)(_random / 2) == 0)//移動した場合相手に当たる可能性がある(Pathのカウント3)の場合50/50で移動か攻撃をする
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
        m_Position = m_PrePosition;
        transform.localPosition = new Vector3(m_PrePosition.x, 0, m_PrePosition.y) + MapManager.Singleton.Offset + Vector3.up;
    }
    public void DamageHP(float damage_)
    {
        m_HP = Mathf.Max(m_HP - damage_, 0.0f);//HP最低値は0,0固定の方がいいやろ...という前提の処理　将来的に変える...？のか？
        if(m_HP == 0.0f)
            m_State = State.Dead;
        Event_DamageHP?.Invoke(m_Index, m_HP);
    }
    public void HealHP(float heal_)
    {
        m_HP = Mathf.Min(m_HP + heal_, m_HPMax);
        Event_HealHP?.Invoke(m_Index, m_HP);
    }

    void Move()
    {
        //経路は[0]が現在地点なので[1]が次のチップ
        transform.localPosition = new Vector3(m_Path[1].x, 0, m_Path[1].y) + MapManager.Singleton.Offset + Vector3.up;
        m_Position = m_Path[1];
        var _dir = m_Position - m_PrePosition;
        transform.rotation = Quaternion.LookRotation(new Vector3(_dir.x, 0, _dir.y), Vector3.up);
    }

    void Attack()
    {
        //自身を抜いた敵のリスト　人数が増えてもこれは基本変わらない
        var enemy = new List<AISystem>(MapManager.Singleton.m_AIManagerList);
        enemy.Remove(this);

        //2人前提で[0]に攻撃判定
        var pos = m_Position - enemy[0].m_Position;
        var flag = false;
        if (pos.x == 0 && Mathf.Abs(pos.y) == 1) flag = true;//前後一マスずれにいるか否か
        else if (Mathf.Abs(pos.x) == 1 && pos.y == 0) flag = true;//左右一マスズレにいるか否か

        if (flag)
            enemy[0].DamageHP(m_Attack);
    }
}