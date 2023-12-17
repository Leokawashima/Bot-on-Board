using System.Collections.Generic;
using UnityEngine;
using Map.Object.Component;

namespace Map.Object
{
    [CreateAssetMenu(fileName = "MO_", menuName = "BoB/Map/MapObject")]
    public class MapObject_SO : ScriptableObject
    {
        public Rarity HasRarity = Rarity.Common;

        public Category HasCategory = Category.Weapon_Type1;

        public string Name = string.Empty;

        [TextArea(3, 5)]
        public string Info = "説明文";

        public Sprite TitleImage;

        public int Cost = 0;

        public bool IsCollider = false;

        public MapObject Prefab;

        [SerializeReference]
        public List<MapObjectComponent> Components = new()
        {
            // データ生成時にデフォルトで追加されてほしいコンポーネントを記述する
            new TurnDestroy(),
        };

        public virtual MapObject Spawn(Vector2Int mapPos_, MapManager manager_)
        {
            var _mo = Instantiate(Prefab, manager_.ObjectParent);
            _mo.transform.localPosition = new Vector3(mapPos_.x, 1.0f, mapPos_.y) + manager_.Offset;
            _mo.transform.localRotation = Prefab.transform.rotation;

            _mo.Data = this;
            _mo.Position = mapPos_;
            foreach (var component in Components)
            {
                var _copy = component.DeepCopyInstance();
                _mo.Components.Add(_copy);
            }

            _mo.Initialize(manager_);

            return _mo;
        }
    }
}