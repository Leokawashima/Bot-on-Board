using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;
using ZXing.QrCode;

/// <summary>
/// リザルトシーンのツイートを管理するクラス
/// </summary>
public class ResultTweet : MonoBehaviour
{
    [SerializeField] TMP_InputField m_tweetInputField;
    [SerializeField] Button m_tweetButton;
    [SerializeField] Button m_tweetForQRButton;
    [SerializeField] RawImage m_rawImage;

    const string
        NEWT_TEXT = "text=",
        NEW_URL = "&url=",
        NEW_HASHTAG = "&hashtags=",
        NEW_LINE = "%0a",
        SPACE = "%20",
        SHARP = "%23";

    // QR画像サイズ
    readonly Vector2Int m_QR_IMAGE_SIZE = new Vector2Int(256, 256);

    // 日本語は2バイト文字なので130字(バイト数で260となる)
    // 通常140字までツイートできるがハッシュタグの差分と余裕をもって130字
    const int TWEET_TEXT_BYTE_SIZE = 260;

    void Start()
    {
        m_tweetInputField.onValidateInput += (string text_, int index_, char add_) =>
        {
            var _encoding = Encoding.GetEncoding("Shift_JIS");
            int _byteSize = _encoding.GetByteCount(text_);
            if (_byteSize == TWEET_TEXT_BYTE_SIZE)
            {
                return '\0';
            }
            return add_;
        };

        m_tweetButton.onClick.AddListener(() =>
        {
            OpenURL(CreateTweetLink(m_tweetInputField.text));
        });
        m_tweetForQRButton.onClick.AddListener(() =>
        {
            m_rawImage.texture = CreateQRCode(CreateTweetLink(m_tweetInputField.text));
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
        return "https://twitter.com/intent/tweet?" // x.comでも機能するっぽい　変える必要ある？イーロンはなぜXに変えたのか
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
        return NEW_LINE + SHARP + name_;
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
                Width = m_QR_IMAGE_SIZE.x,
                Height = m_QR_IMAGE_SIZE.y,

                CharacterSet = "UTF-8",
            }
        };

        var _tex = new Texture2D(m_QR_IMAGE_SIZE.x, m_QR_IMAGE_SIZE.y, TextureFormat.ARGB32, false);
        var _pattern = writer.Write(str_);
        _tex.SetPixels32(_pattern);
        _tex.Apply();

        return _tex;
    }
}