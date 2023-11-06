using UnityEngine;

/// <summary>
/// APIをテストするためのクラス
/// </summary>
public class TestGPT : MonoBehaviour
{
    [SerializeField]
    private bool m_onGPT = false;
    [SerializeField, TextArea, Tooltip("APIキーを入力")]
    private string _useAPIKey;
    [SerializeField, TextArea, Tooltip("ルールを決めてあげる")]
    private string m_rule = "語尾に「にゃ」をつけて";
    [SerializeField, TextArea, Tooltip("送りたいメッセージ")]
    private string m_message = "こんにちは？";

    private async void Start()
    {
        if (m_onGPT)
        {
            var _gpt = new ChatGPTConnection(_useAPIKey, m_rule);
            await _gpt.RequestAsync(m_message);
        }
    }
}
