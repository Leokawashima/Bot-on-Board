using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// RoomのUIを管理するクラス
/// </summary>
public class RoomUIManager : MonoBehaviour
{
    #region Field
    [Header("UI")]
    [SerializeField] GameObject selectUI;
    [SerializeField] GameObject makeRoomUI;
    [SerializeField] GameObject listUI;
    [SerializeField] GameObject hostUI;
    [SerializeField] GameObject clientUI;
    [SerializeField] GameObject connectUI;
    [Header("TextField")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI optionText;
    [SerializeField] Toggle passwardToggle;
    [SerializeField] TextMeshProUGUI passwardText;
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] uint logMax = 20;
    List<string> logStr = new List<string>();

    RoomList roomList;

    #endregion

    void Start()
    {
        logText.text = string.Empty;
        logStr.Clear();

        selectUI.SetActive(true);
        makeRoomUI.SetActive(false);
        listUI.SetActive(false);
        hostUI.SetActive(false);
        clientUI.SetActive(false);
        connectUI.SetActive(false);

        roomList = listUI.GetComponent<RoomList>();
    }

    public void Room_MakeRoom()
    {
        selectUI.SetActive(false);
        makeRoomUI.SetActive(true);
    }
    public void Room_Host()
    {
        selectUI.SetActive(false);
        hostUI.SetActive(true);
        listUI.SetActive(true);

        var buffer = GetRoomData(nameText.text, passwardToggle.isOn, optionText.text);

        LogPush("Host Started");

        RoomManager.Host(buffer);
    }
    public void Room_Client()
    {
        selectUI.SetActive(false);
        clientUI.SetActive(true);
        listUI.SetActive(true);

        LogPush("Client Started");

        void callback(IPEndPoint endP, string data)
        {
            var info = CatchRoomData(endP, data);
            roomList.AddListRoomInfo(info);
        }

        RoomManager.CallBackReceive = callback;
        RoomManager.Client();
    }
    public void Room_Quit()
    {
        RoomManager.Close();

        logText.text = string.Empty;
        logStr.Clear();

        selectUI.SetActive(true);
        makeRoomUI.SetActive(false);
        hostUI.SetActive(false);
        clientUI.SetActive(false);
        listUI.SetActive(false);
    }
    public void Room_Back()
    {
        selectUI.SetActive(true);
        makeRoomUI.SetActive(false);
        hostUI.SetActive(false);
        clientUI.SetActive(false);
        listUI.SetActive(false);
    }
    public void Room_GameStart()
    {

    }
    public void Room_GameReady()
    {
        for(int i = 0, count = roomList.rooms.Count; i < count; ++i)
        {
            LogPush(roomList.rooms[0].address.ToString() + "delete");
            roomList.RemMoveListRoomInfo(roomList.rooms[0]);
        }
    }

    string GetRoomData(string name_, bool passward_, string option_)
    {
        return name_ + "_" + passward_ + "_" + option_;
    }
    string[] CatchRoomData(IPEndPoint endP_, string buffer_)
    {
        //Matching等を使いたかったがOptionの文字まで切り出す可能性がある為SubStringで切り出し
        var data = new string[4];
        data[0] = endP_.Address.ToString();
        data[1] = buffer_.Substring(0, buffer_.IndexOf("_"));//name
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        data[2] = buffer_.Substring(0, buffer_.IndexOf("_"));//passward
        data[3] = buffer_.Substring(buffer_.IndexOf("_") + 1);//option
        return data;
    }
    void LogPush(string msg_)
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

#if UNITY_EDITOR
    [ContextMenu("Test/NonLock")]
    void SendTest()
    {
        var endP = RoomManager.buildEndP;
        var buffer = Encoding.UTF8.GetBytes(GetRoomData("TestSendRoom", false, "hatune miku"));
        RoomManager.Udp?.SendAsync(buffer, buffer.Length, endP);
    }
    [ContextMenu("Test/Lock")]
    void SendTestLocked()
    {
        var endP = RoomManager.buildEndP;
        var buffer = Encoding.UTF8.GetBytes(GetRoomData("TestSendRoom", true, "hatune miku"));
        RoomManager.Udp?.SendAsync(buffer, buffer.Length, endP);
    }
#endif
}
