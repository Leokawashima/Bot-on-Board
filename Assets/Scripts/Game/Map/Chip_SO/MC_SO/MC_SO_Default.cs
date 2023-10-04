using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/MC_SO_Default")]
public class MC_SO_Default : MapChip_SO_Template
{
    public override MapChip MapCreate(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        return base.MapCreate(posdata_, pos_, tf_);
    }
}