using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// このプロジェクトで必要な拡張メソッドをまとめるためのクラス
/// </summary>
/// <remarks>量が増えたり有用なものは別途切り分けてクラスを作る</remarks>
public static class ExtendMethod
{
    /// <summary>
    /// TMP_Textのアルファ値のみを変更する拡張メソッド
    /// </summary>
    /// <param name="text_">対象のTMP_Text</param>
    /// <param name="alpha_">アルファ値</param>
    public static void SetAlpha(this TMP_Text text_, float alpha_)
    {
        text_.color = new Color(text_.color.r, text_.color.g, text_.color.b, alpha_);
    }

    /// <summary>
    /// TMP_Textのアルファ値を加算する拡張メソッド
    /// </summary>
    /// <param name="text_">対象のTMP_Text</param>
    /// <param name="alpha_">アルファ値</param>
    public static void AddAlpha(this TMP_Text text_, float alpha_)
    {
        text_.color = new Color(text_.color.r, text_.color.g, text_.color.b, text_.color.a + alpha_);
    }

    /// <summary>
    /// TMP_Textのアルファ値を減算する拡張メソッド
    /// </summary>
    /// <param name="text_">対象のTMP_Text</param>
    /// <param name="alpha_">アルファ値</param>
    public static void SubAlpha(this TMP_Text text_, float alpha_)
    {
        text_.color = new Color(text_.color.r, text_.color.g, text_.color.b, text_.color.a - alpha_);
    }

    /// <summary>
    /// クラスインスタンスを参照コピーでなく実体コピーするメソッド
    /// </summary>
    /// 参考　https://albatrus.com/entry/2021/07/04/190000
    /// 使いやすくGenerics化した
    /// <typeparam name="T">コピーを行う型</typeparam>
    /// <param name="from_">コピー元</param>
    /// <param name="to_">コピー先　明示的にoutをつける</param>
    public static void DeepCopy<T>(this T from_, out T to_) where T : class
    {
        var _copy = System.Activator.CreateInstance(from_.GetType()) as T;
        var _fields = from_.GetType().GetFields();

        foreach(var field in _fields)
        {
            field.SetValue(_copy, field.GetValue(from_));
        }

        to_ = _copy;
    }

    /// <summary>
    /// クラスインスタンスを参照コピーでなく実体コピーして返すメソッド
    /// </summary>
    /// 参考　https://albatrus.com/entry/2021/07/04/190000
    /// 使いやすくGenerics化した
    /// どうやらenum型はpublicでないとコンパイラが暗黙的に変換するためかprivateではコンパイルできない
    /// <typeparam name="T">コピーを行う型</typeparam>
    /// <param name="from_">コピー元</param>
    /// <returns>コピーしたインスタンス</returns>
    public static T DeepCopyInstance<T>(this T from_) where T : class
    {
        var _copy = System.Activator.CreateInstance(from_.GetType()) as T;
        var _fields = from_.GetType().GetFields();

        foreach (var field in _fields)
        {
            field.SetValue(_copy, field.GetValue(from_));
        }

        return _copy;
    }
}