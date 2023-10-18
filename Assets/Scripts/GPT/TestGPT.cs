using UnityEngine;

/// <summary>
/// APIをテストするためのクラス
/// </summary>
public class TestGPT : MonoBehaviour
{
    [SerializeField]
    bool m_onGPT = false;
    [SerializeField, TextArea, Tooltip("APIキーを入力")]
    string _useAPIKey;
    [SerializeField, TextArea, Tooltip("ルールを決めてあげる")]
    string m_rule = "語尾に「にゃ」をつけて";
    [SerializeField, TextArea, Tooltip("送りたいメッセージ")]
    string m_message = "こんにちは？";

    async void Start()
    {
        if (m_onGPT)
        {
            var _gpt = new ChatGPTConnection(_useAPIKey, m_rule);
            await _gpt.RequestAsync(m_message);
        }
    }
}
