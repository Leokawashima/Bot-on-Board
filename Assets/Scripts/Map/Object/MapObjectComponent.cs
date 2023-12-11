using System;
using UnityEngine;

namespace Map.Object
{
    [Serializable]
    public abstract class MapObjectComponent
    {
        public virtual void Initialize()
        {
        }

        public virtual bool Update()
        {
            return true;
        }

        public virtual void Hit(AISystem ai_)
        {
        }

        public virtual void Destroy()
        {
        }
    }

    public interface IColliderAction
    {
        public bool CheckCollider();
        public void Action();
    }

    public class TurnDestroy : MapObjectComponent
    {
        [Header(nameof(TurnDestroy))]
        [SerializeField] private uint TurnDestruction = 0;
        [SerializeField] private uint TurnMax = 10;
        [SerializeField] private uint TurnSpawn = 10;

        public override void Initialize()
        {
            TurnDestruction = TurnSpawn;
        }
        public override bool Update()
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

    public class Attack : MapObjectComponent, IColliderAction
    {
        [Header(nameof(Attack))]
        [SerializeField] private float Power = 3.0f;

        public virtual bool CheckCollider()
        {
            return true;
        }
        public virtual void Action()
        {
            Debug.Log(Power);
        }
    }

    public class Damage : MapObjectComponent
    {
        [Header(nameof(Damage))]
        [SerializeField] private float Power = 1.0f;

        public override void Hit(AISystem ai_)
        {
            ai_.DamageHP(Power);
        }
    }

    public class Stan : MapObjectComponent
    {
        [Header(nameof(Stan))]
        [SerializeField] private uint StanTurn = 1;

        public override void Hit(AISystem ai_)
        {
            ai_.StanTurn = StanTurn;
        }
    }

    public class Heal : MapObjectComponent
    {
        [Header(nameof(Heal))]
        [SerializeField] private float Power = 1.0f;

        public override void Hit(AISystem ai_)
        {
            ai_.HealHP(Power);
        }
    }

    public class Direction : MapObjectComponent
    {
        [Header(nameof(Direction))]
        [SerializeField] private Vertical VerticalDirection;
        [SerializeField] private Horizontal HorizontalDirection;

        public enum Vertical
        {
            Non = 0,
            Forward = 1,
            Backward = -1,
        }
        public enum Horizontal
        {
            Non = 0,
            Right = 1,
            Left = -1,
        }

        public Vector2Int Vector2D => new((int)HorizontalDirection, (int)VerticalDirection);
        public Vector3Int Vector3D => new((int)HorizontalDirection, 0, (int)VerticalDirection);
    }
}