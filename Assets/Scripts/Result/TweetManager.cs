using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;
using ZXing.QrCode;

public class TweetManager : MonoBehaviour
{
    [SerializeField] TMP_InputField m_TweetTextInputField;
    [SerializeField] Button m_TweetButton;
    [SerializeField] Button m_TweetForQRButton;
    [SerializeField] RawImage m_RawImg;

    const string
        m_NewText = "text=",
        m_NewUrl = "&url=",
        m_NewHashTag = "&hashtags=",
        m_NewLine = "%0a",
        m_Space = "%20",
        m_Sharp = "%23";

    readonly Vector2Int m_Size = new Vector2Int(256, 256);

    void Start()
    {
        m_TweetButton.onClick.AddListener(() =>
        {
            OpenURL(CreateTweetLink(m_TweetTextInputField.text));
        });
        m_TweetForQRButton.onClick.AddListener(() =>
        {
            m_RawImg.texture = CreateQRCode(CreateTweetLink(m_TweetTextInputField.text));
        });
    }

    /// <summary>
    /// リンク文字列を開く　WebGLでの実装予定はないが、仮にWebGLで実装した場合別タブでリンクを開く
    /// 参考　http://negi-lab.blog.jp/Tweet
    /// </summary>
    /// <param name="url_">リンク文字列</param>
    void OpenURL(string url_)
    {
#if UNITY_WEBGL
        Application.ExternalEval(string.Format("window.open('{0}','_blank')", url_));
#else
        Application.OpenURL(url_);
#endif
    }

    /// <summary>
    /// ツイートリンク文字列を返す　今回はこのゲーム専用の文字列を返す
    /// </summary>
    /// <param name="text_">ツイート文字列</param>
    /// <returns>ツイートリンク文字列</returns>
    string CreateTweetLink(string text_)
    {
        return "https://twitter.com/intent/tweet?" // x.comでも機能するっぽいがxes?ツイートの言い換えが分からない　イーロンはなぜXに変えたのか
            + "text=" + text_
            + NL_HashTag("BotonBoard")
            + NL_HashTag("BoB");
    }

    /// <summary>
    /// 改行してハッシュタグを挿入する文字列を返す
    /// </summary>
    /// <param name="name">ハッシュタグ名</param>
    /// <returns>改行タグ文字列</returns>
    string NL_HashTag(string name_)
    {
        return m_NewLine + m_Sharp + name_;
    }

    /// <summary>
    /// 文字列からQRコードテクスチャを生成する
    /// 参考　https://intellectual-curiosity.tokyo/2021/03/06/unity%E3%81%A7qr%E3%82%B3%E3%83%BC%E3%83%89%E3%82%92%E8%AA%AD%E3%81%BF%E5%8F%96%E3%82%8B%E6%96%B9%E6%B3%95/
    /// </summary>
    /// <param name="str_">生成元文字列</param>
    /// <returns>QRコードテクスチャ</returns>
    Texture2D CreateQRCode(string str_)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Width = m_Size.x,
                Height = m_Size.y,

                CharacterSet = "UTF-8",
            }
        };

        var _tex = new Texture2D(m_Size.x, m_Size.y, TextureFormat.ARGB32, false);
        var _pattern = writer.Write(str_);
        _tex.SetPixels32(_pattern);
        _tex.Apply();

        return _tex;
    }
}
