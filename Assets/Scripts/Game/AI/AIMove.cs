using Map;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [Serializable]
    public class AIMove
    {
        public List<Path> Route = new();

        public int Count => Route.Count;

        public void Initialize(AIAgent ai_)
        {
            Route = new();
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