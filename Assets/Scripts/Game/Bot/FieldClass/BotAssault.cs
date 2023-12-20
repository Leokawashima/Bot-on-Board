using System;
using System.Collections.Generic;
using UnityEngine;
using Map.Object.Component;

namespace Bot
{
    [Serializable]
    public class BotAssault : BotField_Template
    {
        [field: SerializeField] public float AttackPower { get; private set; }
        [field: SerializeField] public Weapon Weapon { get; private set; }

        public event Action<BotAgent, Weapon>
            Event_HoldWeapon;
        public event Action<BotAgent>
            Event_ReleaceWeapon;

        public BotAssault(BotAgent bot_) : base(bot_)
        {
            AttackPower = 1.0f; // 仮初期設定
        }

        public void HoldWeapon(Weapon weapon_)
        {
            Weapon = weapon_;
            Event_HoldWeapon?.Invoke(m_operator, weapon_);
        }

        public void ReleaceWeapon()
        {
            Weapon = null;
            Event_ReleaceWeapon?.Invoke(m_operator);
        }

        private void Punch(BotAgent target_)
        {
            target_.Health.Damage(AttackPower);
        }
        private void WeaponAttack(BotAgent target_)
        {
            if (false == Weapon.Attack(target_))
            {
                ReleaceWeapon();
            }
        }

        public void Attack()
        {
            // 自身を抜いた敵のリスト 仲間のAIがいる可能性を考慮できていない
            var _enemy = new List<BotAgent>(BotManager.Singleton.Bots);
            _enemy.Remove(m_operator);

            var _pos = m_operator.Travel.Position;
            for (int i = 0; i < _enemy.Count; ++i)
            {
                var _posRelative = _pos - _enemy[i].Travel.Position;
                
                if (Weapon != null)
                {
                    if (Weapon.CheckCollider(_posRelative))
                    {
                        WeaponAttack(_enemy[i]);
                        break;
                    }
                }
                else
                {
                    // マンハッタン距離法　前後左右一マスかチェック
                    if (1 == (Mathf.Abs(_posRelative.x) + Mathf.Abs(_posRelative.y)))
                    {
                        Punch(_enemy[i]);
                        break;
                    }
                }
            }
        }
    }
}