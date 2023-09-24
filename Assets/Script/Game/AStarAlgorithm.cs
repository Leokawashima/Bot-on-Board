using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm
{
    public int[,] m_MapCost;
    public int m_CostMove = 1;
    public Vector2Int m_MapSize;

    List<Node> m_OpenList = new();

    Vector2Int[] SearchPos { get { return new Vector2Int[4] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) }; } }

    public class Node
    {
        public enum State { Non, Open, Close }
        public State state;
        public Vector2Int pos;
        public Node orizin;
        public int actualCost, estimatedCost, score;

        public void OpenStartNode(Vector2Int start_, Vector2Int target_, int actualCost_, List<Node> list_)
        {
            state = State.Open;
            pos = start_;
            actualCost = actualCost_;
            estimatedCost = Mathf.Abs(target_.x - start_.x + target_.y - start_.y);
            score = actualCost + estimatedCost;

            list_.Add(this);
        }
        public bool OpenNode(Vector2Int coordinate_, Vector2Int start_, Vector2Int target_, int actualCost_, Node orizin_, List<Node> list_)
        {
            if(state != State.Non) return false;

            state = State.Open;
            pos = coordinate_;
            orizin = orizin_;
            actualCost = actualCost_;
            estimatedCost = Mathf.Abs(target_.x - start_.x + target_.y - start_.y);
            score = actualCost + estimatedCost;

            list_.Add(this);
            return coordinate_ == target_;
        }

        public void GetPath(List<Vector2Int> list_)
        {
            list_.Add(pos);

            if(orizin != null)
            {
                orizin.GetPath(list_);
            }
        }
    }

    public AStarAlgorithm(Vector2Int mapSize_, int[,] mapCost_)
    {
        m_MapSize = mapSize_;
        m_MapCost = mapCost_;
    }

    public List<Vector2Int> Search(Vector2Int start_, Vector2Int target_)
    {
        var map = new Node[m_MapSize.y, m_MapSize.x];
        m_OpenList.Clear();

        int cost = 0;

        map[start_.y, start_.x] = new Node();
        map[start_.y, start_.x].OpenStartNode(start_, target_, cost, m_OpenList);

        Node SearchNode()
        {
            while(true)
            {
                cost += m_CostMove;

                if (m_OpenList.Count == 0) return null;

                Node Base = m_OpenList[0];
                for (int i = 1; i < m_OpenList.Count; ++i)
                {
                    if (m_OpenList[i].score < Base.score)
                        Base = m_OpenList[i];
                    else if (m_OpenList[i].actualCost < Base.actualCost)
                        Base = m_OpenList[i];
                }

                for(int i = 0; i < 4; ++i)
                {
                    var pos = Base.pos + SearchPos[i];
                    if(0 <= pos.y && 0 <= pos.x && m_MapSize.y > pos.y && m_MapSize.x > pos.x)
                    {
                        map[pos.y, pos.x] ??= new Node();
                        if(map[pos.y, pos.x].OpenNode(pos, start_, target_, cost + m_MapCost[pos.y, pos.x], Base, m_OpenList))
                            return map[pos.y, pos.x];
                    }
                }

                Base.state = Node.State.Close;
                m_OpenList.Remove(Base);
            }
        }

        List<Vector2Int> path = new();

        SearchNode()?.GetPath(path);
        path.Reverse();

        return path;
    }
}