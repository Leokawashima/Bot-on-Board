using UnityEditor;
using UnityEngine;

// こちらのサイトを参考にインポートした
// https://umistudioblog.com/%E3%80%90unity%E3%80%91hierarchy-%E3%83%93%E3%83%A5%E3%83%BC%E3%82%92%E8%89%B2%E5%88%86%E3%81%91%E3%81%97%E3%81%A6%E8%A6%8B%E3%82%84%E3%81%99%E3%81%8F%E3%81%97%E3%81%9F%E3%81%84%E3%80%90%E7%84%A1/
// ごりごりに改造しているしコメントも個人的な意訳をしているので元のサイトをあてにしたほうがいい
namespace MStudio
{
    [InitializeOnLoad]
    public class StyleHierarchy
    {
        // カラーパレットテーブルのGUID
        private static string[] m_dataArray;
        // カラーパレットテーブルのパス
        private static string m_path;
        // キャッシュフィールド
        private static ColorPalette m_ColorPalette;

        static StyleHierarchy()
        {
            m_dataArray = AssetDatabase.FindAssets("t:ColorPalette");

            if (m_dataArray.Length >= 1)
            {
                // 複数のカラーパレットがある場合GUID[0]のものしか使用しない
                m_path = AssetDatabase.GUIDToAssetPath(m_dataArray[0]);

                m_ColorPalette = AssetDatabase.LoadAssetAtPath<ColorPalette>(m_path);

                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindow;
            }
        }

        private static void OnHierarchyWindow(int instanceID_, Rect selectionRect_)
        {
            // このツールを最初にインポートしたときにエラーがでないように　翻訳あってる？
            //To make sure there is no error on the first time the tool imported in project
            if (m_dataArray.Length == 0) return;

            Object _instance = EditorUtility.InstanceIDToObject(instanceID_);

            if (_instance != null)
            {
                for (int i = 0, count = m_ColorPalette.ColorDesigns.Count; i < count; i++)
                {
                    if (State(_instance, selectionRect_, m_ColorPalette.ColorDesigns[i]))
                    {
                        break;
                    }
                }
            }
        }

        private static bool State(Object instance_, Rect selectionRect_, ColorDesign design_)
        {
            switch (design_.State)
            {
                case ColorDesign.KeyState.Equal:
                    {
                        // 名前が全て一致するか
                        if (instance_.name == design_.KeyName)
                        {
                            var _newName = instance_.name;

                            Draw(_newName, selectionRect_, design_);

                            return true;
                        }
                    }
                    break;
                case ColorDesign.KeyState.Begin:
                    {
                        // 最初に見つけた文字列が一致しているか
                        if (instance_.name.StartsWith(design_.KeyName))
                        {
                            var _newName = instance_.name;

                            Draw(_newName, selectionRect_, design_);

                            return true;
                        }
                    }
                    break;
                case ColorDesign.KeyState.BeginDelete:
                    {
                        // 最初に見つけた文字列が一致しているか
                        if (instance_.name.StartsWith(design_.KeyName))
                        {
                            var _newName = instance_.name;
                            _newName = _newName.Substring(design_.KeyName.Length);

                            Draw(_newName, selectionRect_, design_);

                            return true;
                        }
                    }
                    break;
                case ColorDesign.KeyState.End:
                    {
                        // 最初に見つけた文字列が一致しているか
                        if (instance_.name.EndsWith(design_.KeyName))
                        {
                            var _newName = instance_.name;

                            Draw(_newName, selectionRect_, design_);

                            return true;
                        }
                    }
                    break;
                case ColorDesign.KeyState.EndDelete:
                    {
                        // 最初に見つけた文字列が一致しているか
                        if (instance_.name.EndsWith(design_.KeyName))
                        {
                            var _newName = instance_.name;
                            _newName = _newName.Substring(0, instance_.name.Length - design_.KeyName.Length);

                            Draw(_newName, selectionRect_, design_);

                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        private static void Draw(string name_, Rect selectionRect_, ColorDesign design_)
        {
            switch (design_.Case)
            {
                case ColorDesign.CaseState.Default: break;
                case ColorDesign.CaseState.ToUpper: name_ = name_.ToUpper(); break;
                case ColorDesign.CaseState.ToLower: name_ = name_.ToLower(); break;
            }

            // バックグラウンドに描画して色を設定する
            EditorGUI.DrawRect(selectionRect_, design_.BackGroundColor);

            // カラーパレットの指定通りのスタイルを新しく作る
            var _newStyle = new GUIStyle()
            {
                alignment = design_.TextAlignment,
                fontStyle = design_.FontStyle,
                normal = new GUIStyleState()
                {
                    textColor = design_.TextColor,
                },
            };

            // デフォルトの処理で描画すっけど気に入らねえなら自分でToUpper消してな？(意訳)
            // 改造しているのでそんな必要はない
            EditorGUI.LabelField(selectionRect_, name_, _newStyle);
        }
    }
}