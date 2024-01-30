using System.Collections.Generic;
using UnityEngine;
using Bot;

namespace Map.Object.Component
{
    public class Weapon : MapObjectComponent
    {
        [Header(nameof(Weapon))]
        [SerializeField] protected float Power = 3.0f;
        [SerializeField] protected uint Remain = 1;

        public override void Hit(BotAgent ai_)
        {
            ai_.Assault.HoldWeapon(this.DeepCopyInstance());
        }

        public virtual bool CheckCollider(Vector2Int pos_)
        {
            return (1 == (Mathf.Abs(pos_.x) + Mathf.Abs(pos_.y)));
        }
        public virtual bool Attack(Vector2Int pos_)
        {
            var _bots = BotManager.Singleton.Bots;
            var _hits = new List<BotAgent>(_bots.Count);
            for (int i = 0, cnt = _bots.Count; i < cnt; ++i)
            {
                if (_bots[i].Travel.Position == pos_)
                {
                    _hits.Add(_bots[i]);
                }
            }

            foreach(var hit in _hits)
            {
                hit.Health.Damage(Power);
                if (--Remain <= 0)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class Tools : Weapon
    {
        [SerializeField] private string m_subject;
        [SerializeField] private uint m_toolDamage = 5;

        public override bool CheckCollider(Vector2Int pos_)
        {
            var _hit = base.CheckCollider(pos_);
            var _isRock = false;
            var _obj = MapManager.Singleton.Stage.Object[pos_.y][pos_.x];
            if (_obj != null)
            {
                if (_obj.Data.Name == m_subject)
                {
                    _isRock = true;
                }
            }

            return _hit || _isRock;
        }

        public override bool Attack(Vector2Int pos_)
        {
            var _bots = BotManager.Singleton.Bots;
            var _hits = new List<BotAgent>(_bots.Count);
            for (int i = 0, cnt = _bots.Count; i < cnt; ++i)
            {
                if (_bots[i].Travel.Position == pos_)
                {
                    _hits.Add(_bots[i]);
                }
            }

            foreach (var hit in _hits)
            {
                hit.Health.Damage(Power);
                if (--Remain <= 0)
                {
                    return false;
                }
            }

            var _obj = MapManager.Singleton.Stage.Object[pos_.y][pos_.x];
            if (_obj != null)
            {
                if (_obj.Data.Name == m_subject)
                {
                    var _destroy = _obj.GetMOComponent<TurnDestroy>();
                    _destroy.SubTrun(m_toolDamage);
                }
            }

            return true;
        }
    }

    public class CrossBow : Weapon
    {

    }

    public class LongSword : Weapon
    {
        public override bool CheckCollider(Vector2Int pos_)
        {
            return 1 == Mathf.Abs(pos_.x) || 1 == Mathf.Abs(pos_.y);
        }
    }

    public class Spear : Weapon
    {
        public override bool CheckCollider(Vector2Int pos_)
        {
            return 2 >= Mathf.Abs(pos_.x) + Mathf.Abs(pos_.y);
        }
    }
}
