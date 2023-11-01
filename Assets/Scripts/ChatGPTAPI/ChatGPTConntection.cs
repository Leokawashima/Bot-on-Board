using System;
using System.Text;
using System.Collections.Generic;
// UniTask非同期処理を扱う名前空間
// プロジェクトに自分でUniTaskというGitのパッケージを導入する必要がある
// 参考のURL： https://shibuya24.info/entry/unity-start-unitask
using Cysharp.Threading.Tasks;
using UnityEngine;
// HTTPネットワークを扱う名前空間
// 参考： https://kan-kikuchi.hatenablog.com/entry/UnityWebRequest
using UnityEngine.Networking;

/// <summary>
/// ChatGPTのAPIを使用してやり取りするクラス
/// </summary>
/// 命名規則やクラスを構造体などに変えているが基本は以下から転用
/// 用途に合わせて改善、エラー処理の対策を施していく
/// ソース元URL： https://note.com/negipoyoc/n/n88189e590ac3
public class ChatGPTConnection
{
    // 使用するAPIキー コンストラクタで初期化する
    // APIキーについてはまずこれを読むべし： https://udemy.benesse.co.jp/design/3d/chatgpt-unity.html
    // 少しスクロールするとIキーを発行する方法がのっている
    private readonly string m_API_KEY;

    //会話履歴を保持するリスト
    private readonly List<ChatGPTMessageModel> m_MESSAGE_LIST = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="apiKey_">使用するAPIKey</param>
    /// <param name="content_">ルール設定コンテンツ</param>
    public ChatGPTConnection(string apiKey_, string content_)
    {
        m_API_KEY = apiKey_;
        // 送るメッセージの先頭に文言を追加
        // ここでは role = "system" 要はプロンプトを指定
        // ユーザーの発言ではなく、こういうていで話してねとか、口調で話してとか
        // そういうのをユーザーからお願いするのではなくルールを定義するようにsystemから指定する
        m_MESSAGE_LIST.Add(new ChatGPTMessageModel()
        {
            role = "system",
            content = content_
        });
    }

    /// <summary>
    /// /非同期でAPIのメッセージを受け取りデータ構造体として返す
    /// </summary>
    /// <param name="userMessage_"></param>
    /// <returns>帰ってきた会話データ</returns>
    /// <exception cref="Exception">WebRequestのエラー</exception>
    public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage_)
    {
        // 文章生成AIのAPIのエンドポイントを設定
        var _apiUrl = "https://api.openai.com/v1/chat/completions";

        // ユーザーメッセージを追加
        // 発言ルールを設定できる
        m_MESSAGE_LIST.Add(new ChatGPTMessageModel { role = "user", content = userMessage_ });

        //文章生成で利用するモデルやトークン上限、プロンプトをオプションに設定
        var _options = new ChatGPTCompletionRequestModel()
        {
            model = "gpt-3.5-turbo",
            messages = m_MESSAGE_LIST
        };
        var jsonOptions = JsonUtility.ToJson(_options);

        // OpenAIの文章生成(Completion)にAPIリクエストを送るための構造体
        // usingなのでメソッド終了時に自動的に破棄される
        using var _request = new UnityWebRequest(_apiUrl, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        // OpenAIのAPIリクエストに必要なヘッダー情報を設定
        var _headers = new Dictionary<string, string>
            {
                {"Authorization", "Bearer " + m_API_KEY},
                {"Content-type", "application/json"},
                {"X-Slack-No-Retry", "1"}
            };

        // ヘッダー情報を作成したリクエスト構造体にセット
        foreach(var _header in _headers)
        {
            _request.SetRequestHeader(_header.Key, _header.Value);
        }

        // リクエストを送る(非同期)
        await _request.SendWebRequest();

        // エラーが発生した場合
        if(_request.result == UnityWebRequest.Result.ConnectionError ||
            _request.result == UnityWebRequest.Result.ProtocolError)
        {
#if UNITY_EDITOR
            Debug.LogError(_request.error);
#endif
            throw new Exception();
        }
        // 受け取ったメッセージをJSONから変換し返す
        else
        {
            var _responseJson = _request.downloadHandler.text;
            var _responseObject = JsonUtility.FromJson<ChatGPTResponseModel>(_responseJson);
            // 帰ってきたメッセージをメッセージリストに格納している
            // 要するに会話の履歴を送って会話しているので履歴の数に上限を設けないとすぐTokenがあの世いき
            m_MESSAGE_LIST.Add(_responseObject.choices[0].message);
            return _responseObject;
        }
    }
}

// 発言単位のデータを格納するための構造体
[Serializable]
 public struct ChatGPTMessageModel
{
    public string role;
    public string content;
}

// ChatGPT APIにRequestを送るための構造体
[Serializable]
public struct ChatGPTCompletionRequestModel
{
    public string model;
    public List<ChatGPTMessageModel> messages;
}

// ChatGPT APIからのResponseを受け取るための構造体
[Serializable]
public struct ChatGPTResponseModel
{
    public string id;
    public string @object;
    public int created;
    public Choice[] choices;
    public Usage usage;

    // 会話単位の構造体
    // 恐らくメッセージを再生成したりした時に配列が伸びていく
    [Serializable]
    public struct Choice
    {
        public int index;
        public ChatGPTMessageModel message;
        public string finish_reason;
    }

    // トークン情報
    [Serializable]
    public struct Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }
}