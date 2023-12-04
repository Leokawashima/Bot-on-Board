using System;
using UnityEngine;

namespace Map
{
    // GetType().Name
    // Editor拡張を作成するときはこれで名前を取得して表示を変える
    [Serializable]
    public abstract class MOComponent
    {
    }

    public interface IColliderAction
    {
        public bool CheckCollider();
        public void Action();
    }

    public class Destroy : MOComponent
    {
        [Header(nameof(Destroy))]
        [SerializeField] private uint TurnMax = 10;
        [SerializeField] private uint TurnSpawn = 10;

        public virtual void Initialize()
        {
            Debug.Log(TurnSpawn);
        }
        public virtual void AddTurn(int add_)
        {
            Debug.Log(TurnMax);
        }
    }

    public class Attack : MOComponent, IColliderAction
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

    public class Heal : MOComponent, IColliderAction
    {
        [Header(nameof(Heal))]
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

    public class Direction : MOComponent
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