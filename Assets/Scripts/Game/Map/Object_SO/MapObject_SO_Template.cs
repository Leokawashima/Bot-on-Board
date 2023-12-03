using UnityEngine;

public abstract class MapObject_SO_Template : ScriptableObject
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
        Wweapon_Type2,
        Item,
        Trap,
        Wall,
    }
    public Category HasCategory;

    public string m_ObjectName = string.Empty;
    public string m_Info = "カードにカーソルを合わせたときに表示する説明文";

    [field: SerializeField]
    public int Cost { get; private set; } = 0;

    [field: SerializeField]
    public bool IsCollider { get; private set; } = false;

    public GameObject m_Prefab;

    public virtual MapObject Spawn(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        var go = Instantiate(m_Prefab, tf_.position + pos_, m_Prefab.transform.rotation, tf_);
        var mo = go.AddComponent<MapObject>();
        mo.MapObjectSO = this;

        mo.Position = posdata_;

        return mo;
    }

    public virtual void Destry() { }
}
public interface IMapObject {}

public interface IDestroy : IMapObject
{
    public uint TurnMax { get; set; }
    public uint TurnSpawn { get; set; }
}

public interface IWeapon : IMapObject
{
    public float AttackPower { get; set; }
    public bool CheckAttackCollider();
    public void Attack();
}

public interface IHeal : IMapObject
{
    public float HealPower { get; set; }
    public bool CheckHealCollider();
    public void Heal();
}

public interface IDirection : IMapObject
{
    public enum Vertical
    {
        Non = 0,
        Forward = 1,
        Backward = 2,
    }

    public enum Horizontal
    {
        Non = 0,
        Right = 1,
        Left = 2,
    }

    public Vertical VerticalDirection { get; set; }
    public Horizontal HorizontalDirection { get; set; }
    public Vector2 GetVector()
    {
        return new Vector2((int)HorizontalDirection, (int)VerticalDirection);
    }
}