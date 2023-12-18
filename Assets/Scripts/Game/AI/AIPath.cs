using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [Serializable]
    public class AIPath
    {
        public List<MovePath> Path = new();

        public int Count => Path.Count;

        public void Initialize()
        {
            Path = new();
        }
        public void Clear()
        {
            Path.Clear();
        }

        public void Add(Vector2Int pos_, MoveState state_)
        {
            Path.Add(new MovePath(pos_, state_));
        }

        [Serializable]
        public class MovePath
        {
            public Vector2Int Position { get; private set; }
            public MoveState State { get; private set; }

            public MovePath(Vector2Int position_, MoveState state_)
            {
                Position = position_;
                State = state_;
            }
        }
    }
}