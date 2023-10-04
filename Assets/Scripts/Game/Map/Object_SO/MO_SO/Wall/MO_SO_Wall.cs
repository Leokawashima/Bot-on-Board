using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/Wall")]
public class MO_SO_Wall : MO_Destroy_SO_Template
{
    public override MapObject ObjectSpawn(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        return base.ObjectSpawn(posdata_, pos_, tf_);
    }
}