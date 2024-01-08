using System.Collections.Generic;
using UnityEngine;
using Map.Chip.Component;

namespace Map.Chip
{
    [CreateAssetMenu(fileName = "MC_", menuName = "BoB/Map/MapChip")]
    public class MapChip_SO : ScriptableObject
    {
        public string Name = string.Empty;

        [TextArea(3, 5)]
        public string Info = "説明文";

        public int Cost = 0;

        public MapChip Prefab;

        [SerializeReference]
        public List<MapChipComponent> Components = new()
        {
            // データ生成時にデフォルトで追加されてほしいコンポーネントを記述する
        };

        public virtual MapChip Spawn(Vector2Int pos_, MapManager manager_)
        {
            var _mc = Instantiate(Prefab, manager_.ParentChip);
            _mc.name = $"MC_x:{pos_.x}y:{pos_.y}";
            _mc.transform.localPosition = new Vector3(pos_.x, 0.0f, pos_.y) + manager_.Offset;
            _mc.transform.localRotation = Prefab.transform.rotation;

            _mc.Data = this;
            _mc.Position = pos_;
            foreach (var component in Components)
            {
                var _copy = component.DeepCopyInstance();
                _mc.Components.Add(_copy);
            }

            _mc.Initialize(manager_);

            return _mc;
        }
    }
}