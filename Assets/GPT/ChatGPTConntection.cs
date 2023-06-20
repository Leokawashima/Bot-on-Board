using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTConnection
{
    private readonly string _apiKey;
    //会話履歴を保持するリスト
    private readonly List<ChatGPTMessageModel> _messageList = new();

    public readonly string _content;

    public ChatGPTConnection(string apiKey, string content)
    {
        _apiKey = apiKey;
        _content = content;
        _messageList.Add(
            new ChatGPTMessageModel() { role = "system", content = content });
    }

    public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage)
    {
        //文章生成AIのAPIのエンドポイントを設定
        var apiUrl = "https://api.openai.com/v1/chat/completions";

        _messageList.Add(new ChatGPTMessageModel { role = "user", content = userMessage });

        //OpenAIのAPIリクエストに必要なヘッダー情報を設定
        var headers = new Dictionary<string, string>
            {
                {"Authorization", "Bearer " + _apiKey},
                {"Content-type", "application/json"},
                {"X-Slack-No-Retry", "1"}
            };

        //文章生成で利用するモデルやトークン上限、プロンプトをオプションに設定
        var options = new ChatGPTCompletionRequestModel()
        {
            model = "gpt-3.5-turbo",
            messages = _messageList
        };
        var jsonOptions = JsonUtility.ToJson(options);

        Debug.Log("自分:" + userMessage);

        //OpenAIの文章生成(Completion)にAPIリクエストを送り、結果を変数に格納
        using var request = new UnityWebRequest(apiUrl, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        foreach(var header in headers)
        {
            request.SetRequestHeader(header.Key, header.Value);
        }

        await request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
            throw new Exception();
        }
        else
        {
            var responseString = request.downloadHandler.text;
            var responseObject = JsonUtility.FromJson<ChatGPTResponseModel>(responseString);

            //文字列抜き出しパターン
            string pattern = @"\d+";
            //元の文字列
            Debug.Log("<color=red>ChatGPT:" + responseObject.choices[0].message.content + "</color>");
            //文字列配列として数字のみを記録
            MatchCollection match = Regex.Matches(responseObject.choices[0].message.content, pattern);
            //文字配列を区切り文字指定で連結して一つの文字列に
            string result = string.Join(",", match);
            //正規化された文字列を出力
            Debug.Log("<color=yellow>ChatGPT:" + result + "</color>");

            _messageList.Add(responseObject.choices[0].message);
            return responseObject;
        }
    }
}

[Serializable]
public class ChatGPTMessageModel
{
    public string role;
    public string content;
}

//ChatGPT APIにRequestを送るためのJSON用クラス
[Serializable]
public class ChatGPTCompletionRequestModel
{
    public string model;
    public List<ChatGPTMessageModel> messages;
}

//ChatGPT APIからのResponseを受け取るためのクラス
[System.Serializable]
public class ChatGPTResponseModel
{
    public string id;
    public string @object;
    public int created;
    public Choice[] choices;
    public Usage usage;

    [System.Serializable]
    public class Choice
    {
        public int index;
        public ChatGPTMessageModel message;
        public string finish_reason;
    }

    [System.Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }
}