using System;
using UnityEngine;
using Bot;

namespace Map.Object.Component
{
    [Serializable]
    public abstract class MapObjectComponent
    {
        protected MapObject Object;

        public virtual void Initialize(MapObject obj_)
        {
            Object = obj_;
        }

        public virtual void Update()
        {
        }

        public virtual void Hit(BotAgent bot_)
        {
        }

        public virtual void Destroy()
        {
        }
    }

    public class TurnDestroy : MapObjectComponent
    {
        [Header(nameof(TurnDestroy))]
        [SerializeField] private uint TurnDestruction = 0;
        [SerializeField] private uint TurnMax = 10;
        [SerializeField] private uint TurnSpawn = 10;

        public override void Initialize(MapObject obj_)
        {
            base.Initialize(obj_);
            TurnDestruction = TurnSpawn;
        }
        public override void Update()
        {
            if(--TurnDestruction <= 0)
            {
                Object.WillDestory = true;
            }
        }

        public virtual void AddTurn(uint add_)
        {
            TurnDestruction += add_;
            if(TurnDestruction > TurnMax)
            {
                TurnDestruction = TurnMax;
            }
        }

        public virtual void SubTrun(uint sub_)
        {
            TurnDestruction -= sub_;
        }
    }

    public class Damage : MapObjectComponent
    {
        [Header(nameof(Damage))]
        [SerializeField] private float Power = 1.0f;

        public override void Hit(BotAgent bot_)
        {
            bot_.Health.Damage(Power);
        }
    }

    public class Stan : MapObjectComponent
    {
        [Header(nameof(Stan))]
        [SerializeField] private uint StanTurn = 1;

        public override void Hit(BotAgent bot_)
        {
            bot_.Health.StanTurn += StanTurn;
        }
    }

    public class Heal : MapObjectComponent
    {
        [Header(nameof(Heal))]
        [SerializeField] private float Power = 1.0f;

        public override void Hit(BotAgent bot_)
        {
            bot_.Health.Heal(Power);
        }
    }

    public class Direction : MapObjectComponent
    {
        [Header(nameof(Direction))]
        public DirectionState State;

        public Vector2Int Vector2D
        {
            get
            {
                var _vec = new Vector2Int[]
                {
                    Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
                };
                return _vec[(int)State];
            }
        }
    }

    public class DirectionMove : MapObjectComponent
    {
        [Header(nameof(DirectionMove))]
        // ヘッダーをつけるためだけの空変数
        [SerializeField] private uint Empty;

        public override void Hit(BotAgent bot_)
        {
            bot_.Travel.Step(Object.GetMOComponent<Direction>().Vector2D + Object.Position);
        }
    }

    public class RandomWarp : MapObjectComponent
    {
        [Header(nameof(RandomWarp))]
        // ヘッダーをつけるためだけの空変数
        [SerializeField] private uint Empty;

        public override void Hit(BotAgent bot_)
        {
            var _size = MapManager.Singleton.Stage.Size;
            int _randX = UnityEngine.Random.Range(0, _size.x),
                _randY = UnityEngine.Random.Range(0, _size.y);
            bot_.Travel.Warp(new Vector2Int(_randX, _randY));
        }
    }

    public class IncreaseIntelligent : MapObjectComponent
    {
        [Header(nameof(IncreaseIntelligent))]
        [SerializeField] private uint m_increase = 1;

        public override void Hit(BotAgent bot_)
        {
            bot_.Brain.InceaseIntelligent(m_increase);
        }
    }

    public class DecreaseIntelligent : MapObjectComponent
    {
        [Header(nameof(DecreaseIntelligent))]
        [SerializeField] private uint m_decrease = 1;

        public override void Hit(BotAgent bot_)
        {
            bot_.Brain.DecreaseIntelligent(m_decrease);
        }
    }

    public class Arrow : MapObjectComponent
    {
        [Header(nameof(Arrow))]
        [SerializeField] private uint m_increase = 3;

        public override void Hit(BotAgent bot_)
        {
            bot_.Assault.AddArrow(m_increase);
        }
    }

    public class OverrideSpawn : MapObjectComponent
    {
        [Header(nameof(OverrideSpawn))]
        [SerializeField] private uint Empty;
        public override void Initialize(MapObject obj_)
        {
            Object = obj_;

            var _pos = obj_.Position;
            for (int i = 0, cnt = MapManager.Singleton.MapObjects.Count; i < cnt; ++i)
            {
                if (MapManager.Singleton.MapObjects[i].Position == _pos)
                {
                    MapManager.Singleton.MapObjects[i].Finalize(MapManager.Singleton);
                }
            }
        }
    }

    public class Rock : MapObjectComponent
    {
        [Header(nameof(Rock))]
        [SerializeField] private uint Empty;
    }

    public class Log : MapObjectComponent
    {
        [Header(nameof(Log))]
        [SerializeField] private uint Empty;
    }
}