using System;
using System.Collections.Generic;
using UnityEngine;

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

        public virtual MapChip Spawn(Vector2Int mapPos_, Vector3 pos_, Transform parent_)
        {
            var _mc = Instantiate(Prefab, parent_);
            _mc.transform.localPosition = pos_;
            _mc.transform.localRotation = Prefab.transform.rotation;

            _mc.Data = this;
            _mc.Position = mapPos_;
            foreach (var component in Components)
            {
                var _copy = component.DeepCopyInstance();
                _mc.Components.Add(_copy);
            }

            return _mc;
        }
    }
}