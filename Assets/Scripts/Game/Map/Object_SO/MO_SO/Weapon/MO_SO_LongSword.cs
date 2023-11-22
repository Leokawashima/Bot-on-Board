using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/Weapon/LongSword")]
public class MO_SO_LongSword : MapObject_SO_Template, IDestroy, IWeapon
{
    [field: SerializeField] public uint TurnMax { get; set; }
    [field: SerializeField] public uint TurnSpawn { get; set; }

    [field: SerializeField] public float AttackPower { get; set; }

    bool IWeapon.CheckAttackCollider()
    {
        return true;
    }

    void IWeapon.Attack()
    {

    }
}