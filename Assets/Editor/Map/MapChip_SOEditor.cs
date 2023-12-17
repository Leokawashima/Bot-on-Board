using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Map.Chip.Component;

namespace Map.Chip
{
    /// <summary>
    /// MapObject_SOのInspetor拡張
    /// </summary>
    /// VolumePrifileのAddOverridesのようにコンポーネントを足したくて調べたらあった
    /// 参考　https://light11.hatenadiary.com/entry/2022/08/31/193932
    [CustomEditor(typeof(MapChip_SO))]
    [CanEditMultipleObjects]
    public class MapChip_SOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _label = new GUIContent("Add Components");
            var _style = EditorStyles.miniButtonMid;
            var _rect = GUILayoutUtility.GetRect(_label, _style);
            if(GUI.Button(_rect, _label, _style))
            {
                var _mcSO = target as MapChip_SO;
                // ドロップダウンを表示
                var _dropdown = new MCComponentAdvancedDropdown(new AdvancedDropdownState(), _mcSO);
                _dropdown.Show(_rect);
            }
        }

        /// <summary>
        /// MapChipComponent専用のドロップダウンメニュー
        /// </summary>
        /// AdvancedDropDownItemのIdからどうやってコンポーネントを抽出しようかと思っていたらDictonaryで捌くサイトを見つけてマネした
        /// 参考　https://qiita.com/shogo281/items/fb24cf7d28f06822527e
        public class MCComponentAdvancedDropdown : AdvancedDropdown
        {
            private MapChip_SO m_reference;
            private Dictionary<int, MapChipComponent> m_components = new();

            public MCComponentAdvancedDropdown(AdvancedDropdownState state, MapChip_SO mcSO_) : base(state)
            {
                m_reference = mcSO_;
                minimumSize = new(200.0f, 200.0f);
            }

            protected override AdvancedDropdownItem BuildRoot()
            {
                var _root = new AdvancedDropdownItem("Components");

                var _types = System.Reflection.Assembly.GetAssembly(typeof(MapChipComponent))
                    .GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(MapChipComponent)) && !x.IsAbstract)
                    .ToArray();
                var _components = _types.Select(type => (MapChipComponent)Activator.CreateInstance(type)).ToArray();
                foreach(var component in _components)
                {
                    var _item = new AdvancedDropdownItem(component.GetType().Name);
                    m_components.Add(_item.id, component);
                    _root.AddChild(_item);
                }

                return _root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item_)
            {
                m_reference.Components.Add(m_components[item_.id]);
                EditorUtility.SetDirty(m_reference);
            }
        }
    }
}