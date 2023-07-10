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
    [SerializeField] GameObject loadUI;
    [Header("MakeRoom")]
    [SerializeField] TextMeshProUGUI makeNameText;
    [SerializeField] TextMeshProUGUI makeOptionText;
    [SerializeField] Toggle makePasswardToggle;
    [SerializeField] TextMeshProUGUI makePasswardText;
    [Header("ConnectRoom")]
    [SerializeField] TextMeshProUGUI connectPasswardText;
    [Header("Log")]
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] uint logMax = 20;

    List<string> logStr = new List<string>();

    RoomList roomList;

    enum UIState { Choise, MakeRoom, ConnectRoom, BackRoom, Host, Client }

    #endregion

    void Start()
    {
        logText.text = string.Empty;
        logStr.Clear();

        SetState(UIState.Choise);

        roomList = listUI.GetComponent<RoomList>();
    }
    void OnDestroy()
    {
        RoomManager.Clean();
    }

    public void Room_MakeRoom()
    {
        SetState(UIState.MakeRoom);
    }
    public void Room_ConnectRoom()
    {
        SetState(UIState.ConnectRoom);
    }
    public void Room_BackRoom()
    {
        SetState(UIState.BackRoom);
    }
    public void Room_Host()
    {
        SetState(UIState.Host);

        var buffer = GetRoomData(makeNameText.text, makePasswardToggle.isOn, makeOptionText.text);

        RoomManager.Host(buffer);

        LogPush("Host Started");
    }
    public void Room_Client()
    {
        SetState(UIState.Client);

        void callback(IPEndPoint endP, string data)
        {
            var info = CatchRoomData(endP, data);
            roomList.AddListRoomInfo(info);
        }

        RoomManager.CallBackReceive = callback;
        RoomManager.Client();

        LogPush("Client Started");
    }
    public void Room_Quit()
    {
        SetState(UIState.Choise);

        RoomManager.Close();

        logText.text = string.Empty;
        logStr.Clear();

        for(int i = 0, count = roomList.rooms.Count; i < count; ++i)
        {
            roomList.RemMoveListRoomInfo(roomList.rooms[0]);
        }
    }
    public void Room_Back()
    {
        SetState(UIState.Choise);
    }
    public void Room_GameStart()
    {

    }
    public void Room_GameReady()
    {

    }

    void SetState(UIState state_)
    {
        switch (state_)
        {
            case UIState.Choise:
                selectUI.SetActive(true);
                makeRoomUI.SetActive(false);
                listUI.SetActive(false);
                hostUI.SetActive(false);
                clientUI.SetActive(false);
                connectUI.SetActive(false);
                loadUI.SetActive(false);
                break;
            case UIState.MakeRoom:
                selectUI.SetActive(false);
                makeRoomUI.SetActive(true);
                break;
            case UIState.ConnectRoom:
                connectUI.SetActive(false);
                loadUI.SetActive(true);
                break;
            case UIState.BackRoom:
                connectUI.SetActive(false);
                break;
            case UIState.Host:
                selectUI.SetActive(false);
                hostUI.SetActive(true);
                listUI.SetActive(true);
                break;
            case UIState.Client:
                selectUI.SetActive(false);
                clientUI.SetActive(true);
                listUI.SetActive(true);
                break;
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