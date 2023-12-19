using System;
using System.Collections.Generic;
using UnityEngine;
using Map;
using System.Collections;

namespace AI
{
    [Serializable]
    public class AITravel
    {
        private AIAgent m_operator;

        [field: SerializeField] public Vector2Int Position { get; private set; }
        [field: SerializeField] public Vector2Int PrePosition { get; private set; }

        [field: SerializeField] public List<Path> Route { get; private set; } = new();

        public void Initialize(AIAgent ai_, Vector2Int pos_)
        {
            m_operator = ai_;
            Position = pos_;
            PrePosition = pos_;
        }
        public void Clear()
        {
            Route.Clear();
        }

        public void Step(Vector2Int pos_)
        {
            // コストを持たせて状況に応じて殴らせて移動していくシステムに変更する

            var _mapManager = MapManager.Singleton;
            // 地面がなければムリ
            if (_mapManager.Stage.Chip[pos_.y][pos_.x] == null) return;
            if (_mapManager.Stage.Object[pos_.y][pos_.x] != null)
            {
                // 壁系の当たり判定オブジェクトならムリ
                if (MapManager.Singleton.Stage.Object[pos_.y][pos_.x].Data.IsCollider) return;
            }

            Route.Add(new(pos_, MoveState.Step));
        }
        public void Warp(Vector2Int pos_)
        {
            var _mapManager = MapManager.Singleton;
            // 地面がなければムリ
            if (_mapManager.Stage.Chip[pos_.y][pos_.x] == null) return;
            if (_mapManager.Stage.Object[pos_.y][pos_.x] != null)
            {
                // 壁系の当たり判定オブジェクトならムリ
                if (MapManager.Singleton.Stage.Object[pos_.y][pos_.x].Data.IsCollider) return;
            }

            Route.Add(new(pos_, MoveState.Warp));
        }

        public void BackPosition()
        {
            Position = PrePosition;
            m_operator.transform.localPosition = new Vector3(PrePosition.x, 0, PrePosition.y) + MapManager.Singleton.Offset + Vector3.up;
        }

        public IEnumerator DelayMove()
        {
            for (int i = 0, cnt = Route.Count; i < cnt; ++i)
            {
                if (i != 0)
                {
                    PrePosition = Route[i - 1].Position;
                }
                Position = Route[i].Position;
                switch (Route[i].State)
                {
                    case MoveState.Step:
                        for (int j = 1; j <= 10; ++j)
                        {
                            Vector2 prepos = PrePosition;
                            Vector2 pos = Position;
                            Vector2 _offset = (pos - prepos) * j / 10.0f;
                            m_operator.transform.localPosition = new Vector3(prepos.x, 1, prepos.y)
                                + new Vector3(_offset.x, 0, _offset.y) + MapManager.Singleton.Offset;
                            var _dir = Position - PrePosition;
                            m_operator.transform.rotation = Quaternion.LookRotation(new Vector3(_dir.x, 0, _dir.y), Vector3.up);
                            yield return new WaitForSeconds(0.1f);
                        }
                        break;
                    case MoveState.Warp:
                        {
                            m_operator.transform.localPosition = new Vector3(Position.x, 1, Position.y) + MapManager.Singleton.Offset;
                            yield return new WaitForSeconds(1.0f);
                        }
                        break;
                }

            }
            PrePosition = Position;
        }

        [Serializable]
        public class Path
        {
            [field: SerializeField] public Vector2Int Position { get; private set; }
            [field: SerializeField] public MoveState State { get; private set; }

            public Path(Vector2Int position_, MoveState state_)
            {
                Position = position_;
                State = state_;
            }
        }
    }
}