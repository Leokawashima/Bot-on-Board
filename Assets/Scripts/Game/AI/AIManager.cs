using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public enum Dir_FB { Non = 0, Forward = 1, Back = -1 }
    public enum Dir_RL { Non = 0, Right = 1, Left = -1 }
    public enum State { Non, Alive, Dead }

    public Dir_FB m_Dir_FB { get; private set; } = Dir_FB.Non;
    public Dir_RL m_Dir_RL { get; private set; } = Dir_RL.Non;
    public State m_State { get; private set; } = State.Non;

    public Vector2Int m_Position { get; private set; } = Vector2Int.zero;
    public Vector2Int m_Direction { get { return new Vector2Int((int)m_Dir_RL, (int)m_Dir_FB); } }

    AStarAlgorithm m_AStar;
    [SerializeField] List<Vector2Int> path;

    public void Spawn(string name_, Vector2Int posData_)
    {
        name = name_;
        m_Position = posData_;
        transform.position = new Vector3(posData_.x, 0, posData_.y) + MapManager.Singleton.Offset + Vector3.up;

        var _size = new Vector2Int(MapManager.Singleton.Data_SO.y, MapManager.Singleton.Data_SO.x);
        m_AStar = new AStarAlgorithm(_size, MapManager.Singleton.m_ObjStates);

        MapManager.Singleton.m_AIManagerList.Add(this);
    }

    public void Think()
    {
        var _pos = m_Position;
        
        var enemy = new List<AIManager>(MapManager.Singleton.m_AIManagerList);
        enemy.Remove(this);

        path = m_AStar.Search(_pos, enemy[0].m_Position);
        if(path.Count <= 1) m_State = State.Dead;
    }

    public void Move()
    {
        transform.localPosition = new Vector3(path[1].x, 0, path[1].y)
             + MapManager.Singleton.Offset + Vector3.up;
        m_Position = path[1];
    }

    public void Action()
    {
        
    }
}