using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TEST : MonoBehaviour
{
    [SerializeField]
    bool GPTON = false;
    [SerializeField, TextArea, Tooltip("APIキーを入力")]
    string YourAPIKey;
    [SerializeField, TextArea, Tooltip("ルールを決めてあげる")]
    string YoutContent = "語尾に「にゃ」をつけて";
    [SerializeField, TextArea, Tooltip("送りたいメッセージ")]
    string YourMessage = "こんにちは？";

    async void Start()
    {
        if (GPTON)
        {
            ChatGPTConnection gpt = new ChatGPTConnection(YourAPIKey, YoutContent);
            await gpt.RequestAsync(YourMessage);
        }
    }
}
