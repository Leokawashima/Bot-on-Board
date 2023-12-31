﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Astar探索ロジッククラス
/// </summary>
public class AStarAlgorithm
{
    private MapStateManager m_mapState;
    private int m_MoveCost = 1;

    // ノードを開ける順番　up downのような順だと無限にお互いの場所へ探索できない状態が生まれる
    readonly private Vector2Int[] SearchPosition = new Vector2Int[4]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
    };

    public class Node
    {
        public enum State { Non, Open, Close }
        public State state;
        public Vector2Int positon;
        public Node orizin;
        public int actualCost, estimatedCost, score;

        public void OpenStartNode(Vector2Int start_, Vector2Int target_, int actualCost_, List<Node> list_)
        {
            state = State.Open;
            positon = start_;
            actualCost = actualCost_;
            estimatedCost = Mathf.Abs(target_.x - start_.x + target_.y - start_.y);
            score = actualCost + estimatedCost;

            list_.Add(this);
        }
        public bool OpenNode(Vector2Int coordinate_, Vector2Int start_, Vector2Int target_, int actualCost_, Node orizin_, List<Node> list_)
        {
            if(state != State.Non) return false;

            state = State.Open;
            positon = coordinate_;
            orizin = orizin_;
            actualCost = actualCost_;
            estimatedCost = Mathf.Abs(target_.x - start_.x + target_.y - start_.y);
            score = actualCost + estimatedCost;

            list_.Add(this);
            return coordinate_ == target_;
        }

        public void GetPath(List<Vector2Int> list_)
        {
            list_.Add(positon);

            if(orizin != null)
            {
                orizin.GetPath(list_);
            }
        }
    }

    public AStarAlgorithm(MapStateManager mapState_)
    {
        m_mapState = mapState_;
    }

    private Node SearchNode(Vector2Int orizin_, Vector2Int target_)
    {
        var _nodeMap = new Node[m_mapState.MapSize.y, m_mapState.MapSize.x];
        var _openList = new List<Node>();
        int _cost = 0;

        _nodeMap[orizin_.y, orizin_.x] = new Node();
        _nodeMap[orizin_.y, orizin_.x].OpenStartNode(orizin_, target_, _cost, _openList);

        while(true)
        {
            _cost += m_MoveCost;

            if(_openList.Count == 0) return null;

            // 開く原点を決める
            Node _baseNode = _openList[0];
            for(int i = 1; i < _openList.Count; ++i)
            {
                // スコアが一番低いもの
                if(_openList[i].score < _baseNode.score)
                    _baseNode = _openList[i];
                else if(_openList[i].actualCost < _baseNode.actualCost)
                    _baseNode = _openList[i];
            }

            // 上　右　下　左　の順で探索する　キャラの向きからこの向きで探索すると面白いかも
            for(int i = 0; i < SearchPosition.Length; ++i)
            {
                var _serachPos = _baseNode.positon + SearchPosition[i];
                if(0 <= _serachPos.y && 0 <= _serachPos.x && m_mapState.MapSize.y > _serachPos.y && m_mapState.MapSize.x > _serachPos.x)
                {
                    _nodeMap[_serachPos.y, _serachPos.x] ??= new Node();
                    if(_nodeMap[_serachPos.y, _serachPos.x].OpenNode(_serachPos, orizin_, target_, _cost + m_mapState.MapObjectCost[_serachPos.y, _serachPos.x], _baseNode, _openList))
                        return _nodeMap[_serachPos.y, _serachPos.x];
                }
            }

            _baseNode.state = Node.State.Close;
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