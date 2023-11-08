using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ログメッセージを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class LogSystem : MonoBehaviour
{
    [Header("Attach")]
    [SerializeField] TMP_Text m_logText;
    [Header("Setting")]
    [SerializeField] uint lineMax = 20;

    List<string> m_logMessage = new();

    /// <summary>
    /// ログに行を追加する
    /// </summary>
    /// <param name="message_"></param>
    public void LogPush(string message_)
    {
        // ログメッセージが最大まで入っている場合
        if(m_logMessage.Count == lineMax)
        {
            // 先頭のログを消す
            m_logMessage.RemoveAt(0);
        }

        // メッセージをログに追加する
        m_logMessage.Add(message_);

        // ログの文字列を作成
        string _log = string.Empty;
        foreach(var _message in m_logMessage)
        {
            // メッセージと改行を
            _log += _message + "\n";
        }

        m_logText.text = _log;
    }

    /// <summary>
    /// ログデータをクリアする
    /// </summary>
    public void LogClear()
    {
        m_logText.text = string.Empty;
        m_logMessage.Clear();
    }
}