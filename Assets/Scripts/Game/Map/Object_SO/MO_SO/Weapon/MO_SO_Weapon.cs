using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/ShortSword")]
public class MO_SO_ShortSword : MapObject_SO_Template, IDestroy, IWeapon
{
    [field: SerializeField] public uint TurnMax { get; set; }
    [field: SerializeField] public uint TurnSpawn { get; set; }

    [field: SerializeField] public uint AttackPower { get; set; }

    public bool CheckAttackCollider()
    {
        return true;
    }

    public void Attack()
    {

    }
}