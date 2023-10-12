using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTConnection
{
    const string _apiUrl = "https://api.openai.com/v1/chat/completions";
    readonly string _apiKey;
    readonly List<ChatGPTMessageModel> _messageList;
    Dictionary<string, string> _headers { get{
            return new Dictionary<string, string>
            {
                {"Authorization", "Bearer " + _apiKey},
                {"Content-type", "application/json"},
                {"X-Slack-No-Retry", "1"}
            };}
    }

    public ChatGPTConnection(string apiKey, string content)
    {
        _apiKey = apiKey;
        _messageList = new List<ChatGPTMessageModel>() { new ChatGPTMessageModel() { role = "system", content = content } };
    }

    public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage)
    {
        _messageList.Add(new ChatGPTMessageModel { role = "user", content = userMessage });

        var options = new ChatGPTCompletionRequestModel()
        {
            model = "gpt-3.5-turbo",
            messages = _messageList
        };
        var jsonOptions = JsonUtility.ToJson(options);

        Debug.Log("自分:" + userMessage);

        using var request = new UnityWebRequest(_apiUrl, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        foreach(var header in _headers)
        {
            request.SetRequestHeader(header.Key, header.Value);
        }

        await request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
            throw new Exception();
        }
        else
        {
            var responseString = request.downloadHandler.text;
            var responseObject = JsonUtility.FromJson<ChatGPTResponseModel>(responseString);

            //元の文字列
            Debug.Log("<color=red>ChatGPT:" + responseObject.choices[0].message.content + "</color>");
            //数字に正規化された文字列
            Debug.Log("<color=yellow>ChatGPT:" + StrNomalizeToNumber(responseObject.choices[0].message.content) + "</color>");

            _messageList.Add(responseObject.choices[0].message);
            return responseObject;
        }

        string StrNomalizeToNumber(string str)
        {
            //数字のみに正規化するパターン(詳しくはggr)
            string pattern = @"\d+";
            //数字のみを抜き出した文字配列として返す　実質string[]
            MatchCollection match = Regex.Matches(str, pattern);
            //数字の間にいれる接続文字
            string conection = ",";
            //単体の文字列型に直して返す
            return string.Join(conection, match);
        }
    }
}

#region ChatGPT Models
/// <summary>
/// メッセージ一回単位の構造体
/// </summary>
[Serializable]
public struct ChatGPTMessageModel
{
    public string role;
    public string content;
}

/// <summary>
/// リクエスト送信用の構造体
/// </summary>
[Serializable]
public struct ChatGPTCompletionRequestModel
{
    public string model;
    public List<ChatGPTMessageModel> messages;
}

/// <summary>
/// レスポンスを受け取るための構造体
/// </summary>
[Serializable]
public struct ChatGPTResponseModel
{
    public string id;
    public string @object;
    public int created;
    public Choice[] choices;
    public Usage usage;

    /// <summary>
    /// レスポンスの文字列構造体
    /// </summary>
    [Serializable]
    public struct Choice
    {
        public int index;
        public ChatGPTMessageModel message;
        public string finish_reason;
    }

    /// <summary>
    /// トークン使用量の構造体
    /// </summary>
    [Serializable]
    public struct Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }
}
#endregion