using UnityEngine;

public class TEST : MonoBehaviour
{
    [SerializeField, TextArea, Tooltip("APIキーを入力")]
    string YourAPIKey;
    [SerializeField, TextArea, Tooltip("ルールを決めてあげる")]
    string YoutContent = "語尾に「にゃ」をつけて";
    [SerializeField, TextArea, Tooltip("送りたいメッセージ")]
    string YourMessage = "こんにちは？";

    async void Start()
    {
        ChatGPTConnection gpt = new ChatGPTConnection(YourAPIKey, YoutContent);
        await gpt.RequestAsync(YourMessage);
    }
}
