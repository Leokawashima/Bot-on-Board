using System.Collections.Generic;
using UnityEngine;
using Map;

[CreateAssetMenu(fileName = "MO_", menuName = "Map/MapObject")]
public class MapObject_SO : ScriptableObject
{
    public enum Rarity
    {
        Common,
        UnCommon,
        Rare,
        Epic,
    }
    public Rarity HasRarity = Rarity.Common;

    public enum Category
    {
        Weapon_Type1,
        Weapon_Type2,
        Item,
        Trap,
        Wall,
    }
    public Category HasCategory;

    public string ObjectName = string.Empty;

    [TextArea]
    public string Info = "説明文";

    public int Cost = 0;

    public bool IsCollider = false;

    public MapObject Prefab;

    [SerializeReference]
    public List<MOComponent> Components = new();

    public virtual MapObject Spawn(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        var _mo = Instantiate(Prefab, tf_.position + pos_, Prefab.transform.rotation, tf_);
        _mo.MapObjectSO = this;

        _mo.Position = posdata_;

        return _mo;
    }

    public virtual void Destry()
    {
    }
}