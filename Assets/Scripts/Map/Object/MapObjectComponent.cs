using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Map.Object
{
    [Serializable]
    public abstract class MapObjectComponent
    {
        public virtual void Initialize(MapObject obj_)
        {
        }

        public virtual bool Update(MapObject obj_)
        {
            return true;
        }

        public virtual void Hit(MapObject obj_, AISystem ai_)
        {
        }

        public virtual void Destroy(MapObject obj_)
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
            TurnDestruction = TurnSpawn;
        }
        public override bool Update(MapObject obj_)
        {
            if(--TurnDestruction <= 0)
            {
                return false;
            }
            return true;
        }

        public virtual void AddTurn(uint add_)
        {
            TurnDestruction += add_;
            if(TurnDestruction > TurnMax)
            {
                TurnDestruction = TurnMax;
            }
        }
    }

    public class Weapon : MapObjectComponent
    {
        [Header(nameof(Weapon))]
        [SerializeField] private float Power = 3.0f;
        [SerializeField] private uint Remain = 1;

        public override void Hit(MapObject obj_, AISystem ai_)
        {
            ai_.HasWeapon = this.DeepCopyInstance();
        }

        public virtual bool CheckCollider()
        {
            return true;
        }
        public virtual bool Action(AISystem ai_)
        {
            ai_.DamageHP(Power);
            return --Remain <= 0;
        }
    }

    public class Damage : MapObjectComponent
    {
        [Header(nameof(Damage))]
        [SerializeField] private float Power = 1.0f;

        public override void Hit(MapObject obj_, AISystem ai_)
        {
            ai_.DamageHP(Power);
        }
    }

    public class Stan : MapObjectComponent
    {
        [Header(nameof(Stan))]
        [SerializeField] private uint StanTurn = 1;

        public override void Hit(MapObject obj_, AISystem ai_)
        {
            ai_.StanTurn = StanTurn;
        }
    }

    public class Heal : MapObjectComponent
    {
        [Header(nameof(Heal))]
        [SerializeField] private float Power = 1.0f;

        public override void Hit(MapObject obj_, AISystem ai_)
        {
            ai_.HealHP(Power);
        }
    }

    public class Direction : MapObjectComponent
    {
        [Header(nameof(Direction))]
        public State DirectionState;

        public enum State
        {
            Forward,
            Right,
            Backward,
            Left,
        }

        public Vector2Int Vector2D
        {
            get
            {
                var _vec = new Vector2Int[]
                {
                    Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
                };
                return _vec[(int)DirectionState];
            }
        }
    }

    public class DirectionMove : MapObjectComponent
    {
        [Header(nameof(DirectionMove))]
        [SerializeField] private uint Power;

        public override void Hit(MapObject obj_, AISystem ai_)
        {
            ai_.Move(obj_.GetMOComponent<Direction>().Vector2D + obj_.Position);
        }
    }
}