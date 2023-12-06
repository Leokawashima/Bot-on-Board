using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Map;

/// <summary>
/// MapObject_SOのInspetor拡張
/// </summary>
/// VolumePrifileのAddOverridesのようにコンポーネントを足したくて調べたらあった
/// 参考　https://light11.hatenadiary.com/entry/2022/08/31/193932
[CustomEditor(typeof(MapObject_SO))]
[CanEditMultipleObjects]
public class MapObject_SOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var buttonLabel = new GUIContent("Add Components");
        var buttonStyle = EditorStyles.miniButtonMid;
        var buttonRect = GUILayoutUtility.GetRect(buttonLabel, buttonStyle);
        if (GUI.Button(buttonRect, buttonLabel, buttonStyle))
        {
            var _moSO = target as MapObject_SO;
            // ドロップダウンを表示
            var _dropdown = new MOComponentAdvancedDropdown(new AdvancedDropdownState(), _moSO);
            _dropdown.Show(buttonRect);
        }
    }

    /// <summary>
    /// MOComponent専用のドロップダウンメニュー
    /// </summary>
    /// AdvancedDropDownItemのIdからどうやってコンポーネントを抽出しようかと思っていたらDictonaryで捌くサイトを見つけてマネした
    /// 参考　https://qiita.com/shogo281/items/fb24cf7d28f06822527e
    public class MOComponentAdvancedDropdown : AdvancedDropdown
    {
        private MapObject_SO m_reference;
        private Dictionary<int, MOComponent> m_components = new();

        public MOComponentAdvancedDropdown(AdvancedDropdownState state, MapObject_SO moSO_) : base(state)
        {
            m_reference = moSO_;
            minimumSize = new(200.0f, 200.0f);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var _root = new AdvancedDropdownItem("Components");

            var _types = System.Reflection.Assembly.GetAssembly(typeof(MOComponent))
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(MOComponent)) && !x.IsAbstract)
                .ToArray();
            var _components = _types.Select(type => (MOComponent)Activator.CreateInstance(type)).ToArray();
            foreach (var component in _components)
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
        }
    }
}