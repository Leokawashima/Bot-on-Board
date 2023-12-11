using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Astar探索ロジッククラス
/// </summary>
public class AStarAlgorithm
{
    private readonly MapStateManager m_MAP_STATE;
    private const int MOVE_COST = 1;

    // ノードを開ける順番
    private readonly Vector2Int[] SearchPosition = new Vector2Int[4]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
    };

    public class Node
    {
        public enum NodeState { Non, Open, Close }
        public NodeState State;
        public Vector2Int Position;
        public Node Orizin;
        public int
            ActualCost,
            EstimatedCost,
            Score;

        public void OpenStartNode(Vector2Int start_, Vector2Int target_, int actualCost_, List<Node> list_)
        {
            State = NodeState.Open;
            Position = start_;
            ActualCost = actualCost_;
            EstimatedCost = Mathf.Abs(target_.x - start_.x + target_.y - start_.y);
            Score = ActualCost + EstimatedCost;

            list_.Add(this);
        }
        public bool OpenNode(Vector2Int coordinate_, Vector2Int start_, Vector2Int target_, int actualCost_, Node orizin_, List<Node> list_)
        {
            if (State != NodeState.Non) return false;

            State = NodeState.Open;
            Position = coordinate_;
            Orizin = orizin_;
            ActualCost = actualCost_;
            EstimatedCost = Mathf.Abs(target_.x - start_.x + target_.y - start_.y);
            Score = ActualCost + EstimatedCost;

            list_.Add(this);
            return coordinate_ == target_;
        }

        public void GetPath(List<Vector2Int> list_)
        {
            list_.Add(Position);

            Orizin?.GetPath(list_);
        }
    }

    public AStarAlgorithm(MapStateManager mapState_)
    {
        m_MAP_STATE = mapState_;
    }

    private Node SearchNode(Vector2Int orizin_, Vector2Int target_)
    {
        var _nodeMap = new Node[m_MAP_STATE.MapSize.y][];
        for (int i = 0; i < m_MAP_STATE.MapSize.y; i++)
            _nodeMap[i] = new Node[m_MAP_STATE.MapSize.x];

        var _openList = new List<Node>();
        int _cost = 0;

        _nodeMap[orizin_.y][orizin_.x] = new Node();
        _nodeMap[orizin_.y][orizin_.x].OpenStartNode(orizin_, target_, _cost, _openList);

        while (true)
        {
            _cost += MOVE_COST;

            if (_openList.Count == 0) return null;

            // 開く原点を決める
            Node _baseNode = _openList[0];
            for (int i = 1; i < _openList.Count; ++i)
            {
                // スコアが一番低いならベースに置き換える
                if (_openList[i].Score < _baseNode.Score)
                {
                    _baseNode = _openList[i];
                    continue;
                }
                // 
                if (_openList[i].ActualCost < _baseNode.ActualCost)
                {
                    _baseNode = _openList[i];
                    continue;
                }
            }

            // 上　右　下　左　の順で探索する　キャラの向きからこの向きで探索すると面白いかも
            for (int i = 0; i < SearchPosition.Length; ++i)
            {
                var _searchPos = _baseNode.Position + SearchPosition[i];
                if (0 <= _searchPos.y && 0 <= _searchPos.x)
                {
                    if (m_MAP_STATE.MapSize.y > _searchPos.y && m_MAP_STATE.MapSize.x > _searchPos.x)
                    {
                        _nodeMap[_searchPos.y][_searchPos.x] ??= new Node();
                        var _searchCost = _cost;

                        if (m_MAP_STATE.MapChips[_searchPos.y][_searchPos.x] != null)
                        {
                            _searchCost += m_MAP_STATE.MapChips[_searchPos.y][_searchPos.x].Data.Cost;
                        }
                        
                        if (m_MAP_STATE.MapObjects[_searchPos.y][_searchPos.x] != null)
                        {
                            _searchCost += m_MAP_STATE.MapObjects[_searchPos.y][_searchPos.x].Data.Cost;
                        }

                        if (_nodeMap[_searchPos.y][_searchPos.x].OpenNode(
                            _searchPos,// 座標
                            orizin_, target_,// 開始地点、目標地点
                            _searchCost,// 検索にかかるコスト
                            _baseNode, _openList))// 検索元のノード、現在空いているノードのリスト
                            return _nodeMap[_searchPos.y][_searchPos.x];
                    }
                }
            }

            _baseNode.State = Node.NodeState.Close;
            _openList.Remove(_baseNode);
        }
    }

    public List<Vector2Int> Search(Vector2Int orizin_, Vector2Int target_)
    {
        List<Vector2Int> _path = new();

        SearchNode(orizin_, target_)?.GetPath(_path);
        _path.Reverse();

        return _path;
    }
}