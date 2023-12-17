using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// TMP_Textをレインボーに変化させるクラス
/// </summary>
public class RainbowText : CorutineMonoBehaivour
{
    /// <summary>
    /// 色を変化させるテキスト
    /// </summary>
    [SerializeField]
    private TMP_Text m_text;

    /// <summary>
    /// 最初に設定される色
    /// </summary>
    [SerializeField]
    private Color m_startColor = Color.red;

    /// <summary>
    /// 変化する速度
    /// </summary>
    [SerializeField]
    private float m_speed = 1.0f;

    protected override IEnumerator CoProcess()
    {
        // 最初の色をHSV形式に変換して彩度のみ抽出
        Color.RGBToHSV(m_startColor, out float _h, out _, out _);
        while (true)
        {
            m_text.color = Color.HSVToRGB((_h + Time.time * m_speed) % 1.0f, 1.0f, 1.0f);
            yield return null;
        }
    }
}