using System.Collections.Generic;
using UnityEngine;
using Map;

[CreateAssetMenu(fileName = "MO_", menuName = "Map/MapObject")]
public class MapObject_SO : ScriptableObject
{
    public Rarity HasRarity = Rarity.Common;

    public Category HasCategory = Category.Weapon_Type1;

    public string ObjectName = string.Empty;

    [TextArea(3, 5)]
    public string Info = "説明文";

    public int Cost = 0;

    public bool IsCollider = false;

    public MapObject Prefab;

    [SerializeReference]
    public List<MOComponent> Components = new()
    {
        // SO生成時にデフォルトで追加されてほしいコンポーネントを記述する
        new TurnDestroy(),
    };

    public virtual MapObject Spawn(Vector2Int mapPos_, Vector3 pos_, Transform parent_)
    {
        var _mo = Instantiate(Prefab, parent_);
        _mo.transform.localPosition = pos_;
        _mo.transform.localRotation = Prefab.transform.rotation;

        _mo.Data = this;
        _mo.Position = mapPos_;
        foreach(var component in Components)
        {
            _mo.Components.Add(component);
        }

        return _mo;
    }
}