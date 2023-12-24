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

        public virtual MapObject Spawn(Vector2Int pos_, MapManager manager_)
        {
            var _mo = Instantiate(Prefab, manager_.ObjectParent);
            _mo.name = $"MO_x:{pos_.x}y:{pos_.y}";
            _mo.transform.localPosition = new Vector3(pos_.x, 1.0f, pos_.y) + manager_.Offset;
            _mo.transform.localRotation = Prefab.transform.rotation;

            _mo.Data = this;
            _mo.Position = pos_;
            foreach (var component in Components)
            {
                var _copy = component.DeepCopyInstance();
                _mo.Components.Add(_copy);
            }

            _mo.Initialize(manager_);

            return _mo;
        }

        public T GetMOComponent<T>() where T : MapObjectComponent
        {
            foreach (var component in Components)
            {
                if (component.GetType() == typeof(T))
                {
                    return component as T;
                }
            }
            return null;
        }
    }
}