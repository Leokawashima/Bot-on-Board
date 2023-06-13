namespace Learning
{
    //ある程度の参考情報を乗っけておく
    //ソース元URL： https://note.com/negipoyoc/n/n88189e590ac3

    using System;//ExceptionとSystem.Serializableの名前空間
    using System.Collections.Generic;//List等のGenericsの名前空間
    using System.Text;//SJISがデフォルトなのでUTF-8変換するためのエンコードに使う文字エンコード名前空間 

    using Cysharp.Threading.Tasks;
    //これが一番重要　UniTaskというコルーチンなどの非同期処理を扱う名前空間
    //プロジェクトに自分でUniTaskというGitのパッケージを導入する必要がある
    //参考のURL： https://shibuya24.info/entry/unity-start-unitask

    using UnityEngine;
    using UnityEngine.Networking;//HTTPネットワークを扱う名前空間　参考： https://kan-kikuchi.hatenablog.com/entry/UnityWebRequest

    public class ChatGPTConnection
    {
        private readonly string _apiKey;
        //コンストラクタで自身のAPIキーを入れて初期化を行う
        //readonlyとは一回初期化したら以降は読み取りしかしないよというのを明示したアクセス修飾子
        //APIキーが分からない人はまずこれを読むべし： https://udemy.benesse.co.jp/design/3d/chatgpt-unity.html
        //少しスクロールしたら自身のアカウントからAPIキーを発行する方法がのっている

        private readonly List<ChatGPTMessageModel> _messageList = new();
        //会話履歴を保持するリスト
        //Listはもちろん分かる...よね C言語でやってるVectorみたいな動的配列と思えばよい

        //コンストラクタ
        public ChatGPTConnection(string apiKey)
        {
            //初期化
            _apiKey = apiKey;
            //送るメッセージの先頭に文言を追加
            //ここでは role = "system" で要はプロンプトを指定している
            //ユーザーの発言ではなく、こういうていで話してねとか、口調で話してとか。
            //そういうのをユーザーからお願いするのではなくルールを定義するようにsystemから指定する
            _messageList.Add(
                new ChatGPTMessageModel() { role = "system", content = "語尾に「にゃ」をつけて" });
        }

        //非同期動作をする関数
        public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage)
        {
            //文章生成AIのAPIのエンドポイントを設定
            var apiUrl = "https://api.openai.com/v1/chat/completions";

            //ユーザーメッセージを追加
            //system 発言ルール
            //user GPTに言うこと
            //今んとここの内容が格納されている
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

            //エラーが発生した場合
            if(request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                throw new Exception();
            }
            //受け取ったメッセージをJSONから変換してデバッグログで出力し
            else
            {
                var responseString = request.downloadHandler.text;
                var responseObject = JsonUtility.FromJson<ChatGPTResponseModel>(responseString);
                Debug.Log("ChatGPT:" + responseObject.choices[0].message.content);
                _messageList.Add(responseObject.choices[0].message);
                return responseObject;
            }
        }
    }

    //一発言毎のデータを格納するための構造体
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
    [Serializable]
    public class ChatGPTResponseModel
    {
        public string id;
        public string @object;
        public int created;
        public Choice[] choices;
        public Usage usage;

        [Serializable]
        public class Choice
        {
            public int index;
            public ChatGPTMessageModel message;
            public string finish_reason;
        }

        [Serializable]
        public class Usage
        {
            public int prompt_tokens;
            public int completion_tokens;
            public int total_tokens;
        }
    }
}