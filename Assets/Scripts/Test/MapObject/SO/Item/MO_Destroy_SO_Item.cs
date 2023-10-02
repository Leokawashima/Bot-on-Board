using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/Destroy_Item")]
public class MO_Destroy_SO_Item : MO_Destroy_SO_Template
{
    public override MapObject ObjectSpawn(Vector2Int pos_, Transform tf_)
    {
        return base.ObjectSpawn(pos_, tf_);
    }
}