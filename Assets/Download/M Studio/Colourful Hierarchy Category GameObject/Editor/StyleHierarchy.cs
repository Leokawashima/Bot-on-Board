using UnityEditor;
using UnityEngine;

//������̃T�C�g���Q�l�ɃC���|�[�g����
//https://umistudioblog.com/%E3%80%90unity%E3%80%91hierarchy-%E3%83%93%E3%83%A5%E3%83%BC%E3%82%92%E8%89%B2%E5%88%86%E3%81%91%E3%81%97%E3%81%A6%E8%A6%8B%E3%82%84%E3%81%99%E3%81%8F%E3%81%97%E3%81%9F%E3%81%84%E3%80%90%E7%84%A1/
//Key��������������d�g�݂�K��ToUpper�ő啶���ɂ���d�g�݂Ȃǎ኱���񂵂������̂Ōl�I�ɉ��ς��Ă���
//��̓I�ɂ�45�s�ڂ�50�s�ڂ�71�s��
namespace MStudio
{
    [InitializeOnLoad]
    public class StyleHierarchy
    {
        static string[] dataArray;//Find ColorPalette GUID
        static string path;//Get ColorPalette(ScriptableObject) path
        static ColorPalette colorPalette;

        static StyleHierarchy()
        {
            dataArray = AssetDatabase.FindAssets("t:ColorPalette");

            if (dataArray.Length >= 1)
            {    //We have only one color palette, so we use dataArray[0] to get the path of the file
                path = AssetDatabase.GUIDToAssetPath(dataArray[0]);

                colorPalette = AssetDatabase.LoadAssetAtPath<ColorPalette>(path);

                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindow;
            }
        }

        private static void OnHierarchyWindow(int instanceID, Rect selectionRect)
        {
            //To make sure there is no error on the first time the tool imported in project
            if (dataArray.Length == 0) return;

            UnityEngine.Object instance = EditorUtility.InstanceIDToObject(instanceID);

            if (instance != null)
            {
                for (int i = 0; i < colorPalette.colorDesigns.Count; i++)
                {
                    var design = colorPalette.colorDesigns[i];

                    //Check if the name of each gameObject is begin with keyChar in colorDesigns list.
                    #region ���ωӏ�
                    //instance.name.StartsWith(design.keyChar)���疼�O����v������̂ɕύX
                    if(instance.name == design.keyChar)
                    #endregion
                    {
                        #region ���ωӏ�
                        //Remove the symbol(keyChar) from the name.
                        string newName = instance.name;
                        //newName = newName.Substring(design.keyChar.Length);
                        //newName = newName.ToUpper();
                        #endregion

                        //Draw a rectangle as a background, and set the color.
                        EditorGUI.DrawRect(selectionRect, design.backgroundColor);

                        //Create a new GUIStyle to match the desing in colorDesigns list.
                        GUIStyle newStyle = new GUIStyle
                        {
                            alignment = design.textAlignment,
                            fontStyle = design.fontStyle,
                            normal = new GUIStyleState()
                            {
                                textColor = design.textColor,
                            }
                        };

                        #region ���ωӏ�
                        //Draw a label to show the name in upper letters and newStyle.
                        //If you don't like all capital latter, you can remove ".ToUpper()".
                        EditorGUI.LabelField(selectionRect, newName, newStyle);
                        #endregion
                    }
                }
            }
        }
    }
}