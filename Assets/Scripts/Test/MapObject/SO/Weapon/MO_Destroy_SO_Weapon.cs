using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/Destroy_Weapon")]
public class MO_Destroy_SO_Weapon : MO_Destroy_SO_Template
{
    public override MapObject ObjectSpawn(Vector2Int pos_, Transform tf_)
    {
        return base.ObjectSpawn(pos_, tf_);
    }
}