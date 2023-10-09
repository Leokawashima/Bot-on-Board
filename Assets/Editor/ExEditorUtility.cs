using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor上でのみ機能する便利関数を取りまとめるクラス
/// </summary>
public static class ExEditorUtility
{
    /// <summary>
    /// 呼び出し元のクラスのスクリプトが存在するパスを返す
    /// </summary>
    /// <typeparam name="T">パスが欲しいクラス</typeparam>
    /// <returns>パス文字列</returns>
    /// GPTにスクリプトから自身ないし特定クラスのスクリプトへのパスを取得する方法はないか？と聞いて帰ってきたのを
    /// 改良してジェネリック追加メソッドにしてみたもの　恐らく参考元になったソースがネット上のどこかにあるはず
    public static string GetScriptPath<T>() where T : class
    {
        MonoScript _script = MonoScript.FromScriptableObject(ScriptableObject.CreateInstance(typeof(T)));
        var _path = AssetDatabase.GetAssetPath(_script);
        var _subStr = $"/{typeof(T)}.cs";
        //_path[0..?]という範囲演算子というものを知ったが C＃ 8.0～からのようなので前バージョンでも動き明示的なRemove採用
        var _folderPath = _path.Remove(_path.Length - _subStr.Length);
        return _folderPath;
    }

    /// <summary>
    /// 呼び出し元のクラスのスクリプトが存在するパスを返す
    /// </summary>
    /// <typeparam name="T">パスが欲しいクラス</typeparam>
    /// <param name="_">パスが欲しいクラスのインスタンス</param>
    /// <returns>パス文字列</returns>
    public static string GetScriptPath<T>(this T _) where T : class
    {
        return GetScriptPath<T>();
    }
}