using UnityEngine;
using UnityEngine.UI;

public abstract class MapObject_SO_Template : ScriptableObject
{
    public enum Rarity
    {
        common = 0,
        unCommon = 1,
        Rare = 2,
        Epic = 3,
    }
    public Rarity hasRarity = Rarity.common;

    public string m_ObjectName = string.Empty;
    public string m_Info = "カードにカーソルを合わせたときに表示する説明文";

    [field: SerializeField]
    public int Cost { get; private set; } = 0;

    [field: SerializeField]
    public bool IsCollider { get; private set; } = false;

    public GameObject m_Prefab;
    public MapObjectCard m_Card;

    const float CardSelectOffset = 50.0f;

    public virtual MapObject Spawn(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        var go = Instantiate(m_Prefab, tf_.position + pos_, m_Prefab.transform.rotation, tf_);
        var mo = go.AddComponent<MapObject>();
        mo.MapObjectSO = this;

        mo.Position = posdata_;

        return mo;
    } 
    public virtual MapObjectCard CardCreate(int index_, Transform tf_, ToggleGroup group_, CardManager cardManager_)
    {
        var moc = Instantiate(m_Card, tf_);
        moc.m_SO = this;
        moc.m_Index = index_;
        moc.m_Text.text = m_ObjectName;
        moc.m_Toggle.group = group_;
        moc.m_CardManager = cardManager_;
        moc.m_Toggle.onValueChanged.AddListener((bool isOn_) =>
        {
            var rect = moc.transform as RectTransform;
            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x,
                isOn_ ?
                rect.anchoredPosition.y + CardSelectOffset : rect.anchoredPosition.y - CardSelectOffset
                );
        });

        return moc;
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