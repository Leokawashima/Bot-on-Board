using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/Weapon/IronPipe")]
public class MO_SO_IronPipe : MapObject_SO_Template, IDestroy, IWeapon
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