using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomLogManager : MonoBehaviour
{
    [Header("Log")]
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] uint logMax = 20;

    List<string> logStr = new List<string>();

    /// <summary>
    /// ログに行を追加する
    /// </summary>
    public void LogPush(string msg_)
    {
        if(logStr.Count == logMax)
        {
            logStr.RemoveAt(0);
        }
        logStr.Add(msg_);

        logText.text = null;
        foreach(var item in logStr)
        {
            logText.text += item;
            logText.text += "\n";
        }
    }

    public void LogClear()
    {
        logText.text = string.Empty;
        logStr.Clear();
    }
}
