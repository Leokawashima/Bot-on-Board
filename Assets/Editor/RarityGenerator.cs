using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// レアリティーとカテゴリーをデータから生成するクラス
/// </summary>
/// 参考　https://www.hanachiru-blog.com/entry/2019/12/20/221633
public class RarityGenerator
{
    [MenuItem("BoB/Generate Rarity")]
    private static void Generate()
    {
        var _code = 
@"
public class AutoGenerateCode
{
    public void Test()
    {
        int a = 0;
    }
}
";
        var _defPath = "Assets/Rarity.cs";
        var _path = AssetDatabase.GenerateUniqueAssetPath(_defPath);
        var _sw = new StreamWriter(_path);
        _sw.Write(_code);
        _sw.Flush();
        _sw.Close();
        AssetDatabase.Refresh();
    }
}