using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public MapManager m_MapManager;
    public Vector2Int m_SpawnPosition = Vector2Int.zero;

    public enum Dir_FB { Non = 0, Forward = 1, Back = -1 }
    public enum Dir_RL { Non = 0, Right = 1, Left = -1 }
    public enum State { Non, Alive, Dead }

    public Dir_FB m_Dir_FB { get; private set; } = Dir_FB.Non;
    public Dir_RL m_Dir_RL { get; private set; } = Dir_RL.Non;
    public State m_State { get; private set; } = State.Non;

    public Vector2Int m_Position { get; private set; } = Vector2Int.zero;
    public Vector2Int m_Direction { get { return new Vector2Int((int)m_Dir_RL, (int)m_Dir_FB); } }

    public AIManager[] m_Target { get; private set; }

    AStarAlgorithm m_AStar;
    [SerializeField] List<Vector2Int> path;

    public void Spawn()
    {
        var _size = new Vector2Int(m_MapManager.Data_SO.y, m_MapManager.Data_SO.x);
        m_AStar = new AStarAlgorithm(_size, m_MapManager.m_ObjStates);
    }

    public void Think()
    {
        var _pos = m_SpawnPosition;

        path = m_AStar.Search(_pos, m_Target[0].m_Position);
        if(path.Count <= 1) m_State = State.Dead;
    }

    public void Move()
    {
        transform.localPosition = new Vector3(path[1].x, 0, path[1].y);
        m_Position = path[1];
    }

    public void Action()
    {
        
    }
}