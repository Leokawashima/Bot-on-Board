using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/Wall")]
public class MO_SO_Wall : MO_Destroy_SO_Template
{
    public override MapObject ObjectSpawn(Vector2Int pos_, Transform tf_)
    {
        return base.ObjectSpawn(pos_, tf_);
    }
}