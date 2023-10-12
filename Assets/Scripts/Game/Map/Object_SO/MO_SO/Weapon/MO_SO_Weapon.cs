using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/ShortSword")]
public class MO_SO_ShortSword : MO_SO_Weapon
{
    public override bool CheckAttackCollider()
    {
        return true;
    }

    public override void Attack()
    {

    }
}