using UnityEngine;
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
}