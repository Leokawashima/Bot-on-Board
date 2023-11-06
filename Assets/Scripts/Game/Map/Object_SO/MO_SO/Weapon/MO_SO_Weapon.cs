using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/ShortSword")]
public class MO_SO_ShortSword : MapObject_SO_Template, IWeapon
{
    [field: SerializeField] public uint AttackPower { get; set; } = 1;

    public bool CheckAttackCollider()
    {
        return true;
    }

    public void Attack()
    {

    }
}