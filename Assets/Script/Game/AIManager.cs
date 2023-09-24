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

    public AIManager m_Target { get; private set; }

    public void Spawn()
    {
        //m_MapManager.objStates[m_SpawnPosition.y, m_SpawnPosition.x] = (int)MapManager.ObjState.AI;
    }

    public void Move()
    {
        //m_MapManager.objStates[m_SpawnPosition.y, m_SpawnPosition.x] = (int)MapManager.ObjState.Non;
        //m_MapManager.objStates[m_SpawnPosition.y + m_Direction.y, m_SpawnPosition.x + m_Direction.x] = (int)MapManager.ObjState.AI;
    }

    public void Think()
    {

    }

    public void Action()
    {
        
    }
}