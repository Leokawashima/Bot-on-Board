using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// TMP_Textをレインボーに変化させるクラス
/// </summary>
public class RainbowTextSystem : MonoBehaviour
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

    /// <summary>
    /// コルーチンのアクティブなものを保持するフィールド
    /// </summary>
    private Coroutine m_activeCorutine;

    /// <summary>
    /// レインボーさせる
    /// </summary>
    public void Rainbow()
    {
        m_activeCorutine = StartCoroutine(CoRainbow());
    }

    /// <summary>
    /// レインボーを停止する
    /// </summary>
    public void Stop()
    {
        // ヌルなら例外がスローされるため独自に例外処理を組み込む必要はない
        StopCoroutine(m_activeCorutine);
        m_activeCorutine = null;
    }

    /// <summary>
    /// レインボーを行うコルーチン
    /// </summary>
    private IEnumerator CoRainbow()
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