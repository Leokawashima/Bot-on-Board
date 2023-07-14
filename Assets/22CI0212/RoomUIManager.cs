using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Sockets;

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

    ListUIManager listUIManager;

    enum UIState { Choise, MakeRoom, ConnectRoom, BackRoom, Host, Client }
    enum NetState { Error = -1, Room, ConnectRequire, ConnectResponse = 2 }

    #endregion

    void Start()
    {
        logText.text = string.Empty;
        logStr.Clear();

        SetUI(UIState.Choise);

        listUIManager = listUI.GetComponent<ListUIManager>();
        listUIManager.InitializeList();

        void callbackHost(IPEndPoint endP_, string buffer_)
        {
            switch(CheckNetState(ref buffer_))
            {
                case NetState.ConnectRequire:
                    {
                        var data = Catch_RequireData(endP_, buffer_);
                        var flag = !makePasswardToggle.isOn && data[2] == makePasswardText.text;
                        if(flag)
                        {
                            listUIManager.AddListMemberInfo(endP_.Address, data);
                            LogPush(data[1] + " joined");
                        }

                        var message = Get_ResponseData(flag);
                        RoomManager.HostMessage(message, endP_.Address);
                    }
                    break;
            }
        }
        RoomManager.CallBack_ReceiveHost = callbackHost;

        void callbackClient(IPEndPoint endP_, string buffer_)
        {
            switch(CheckNetState(ref buffer_))
            {
                case NetState.Room:
                    {
                        var data = Catch_OpenData(endP_, buffer_);
                        if(endP_.Address != RoomManager.GetLocalIPAddress())
                            listUIManager.AddListRoomInfo(endP_.Address, data);
                    }
                    break;
                case NetState.ConnectResponse:
                    {
                        var data = Catch_ResponseData(endP_, buffer_);
                        Debug.Log(data[1]);
                        RoomManager.Close();
                        if(bool.Parse(data[1]))
                        {
                            LogPush("Conected");
                            SetUI(UIState.Client);
                            for(int i = 0, count = listUIManager.rooms.Count; i < count; ++i)
                            {
                                listUIManager.RemoveListRoomInfo(listUIManager.rooms[i]);
                            }
                            var hostdata = new string[2] { data[0], "Host" };
                            listUIManager.AddListMemberInfo(endP_.Address, hostdata);
                            var mydata = new string[2] { RoomManager.GetLocalIPAddress().ToString(), "me" };
                            listUIManager.AddListMemberInfo(endP_.Address, mydata);
                        }
                        else
                        {
                            Room_Client();
                        }
                    }
                    break;
            }
        }
        RoomManager.CallBack_ReceiveClient = callbackClient;
    }
    void OnDestroy()
    {
        RoomManager.Clean();
    }

    public void Room_MakeRoom()
    {
        SetUI(UIState.MakeRoom);
    }
    public void Room_ConnectRequire()
    {
        SetUI(UIState.ConnectRoom);

        RoomManager.Close();

        var buffer = Get_RequireData("Name", makePasswardText.text);
        RoomManager.Connect(buffer, listUIManager.selectRoom.roomAddress);

        LogPush("Connect Require");
    }
    public void Room_BackRoom()
    {
        SetUI(UIState.BackRoom);
    }
    public void Room_Host()
    {
        SetUI(UIState.Host);

        var buffer = Get_OpenData(makeNameText.text, makePasswardToggle.isOn, makeOptionText.text);
        RoomManager.Host(buffer);

        var data = new string[2] { RoomManager.GetLocalIPAddress().ToString(), "Host" };
        listUIManager.AddListMemberInfo(RoomManager.GetLocalIPAddress(), data);

        LogPush("Host Started");
    }
    public void Room_Client()
    {
        SetUI(UIState.Client);

        RoomManager.Client();

        LogPush("Client Started");
    }
    public void Room_Quit()
    {
        SetUI(UIState.Choise);

        RoomManager.Close();

        logText.text = string.Empty;
        logStr.Clear();

        for(int i = 0, count = listUIManager.rooms.Count; i < count; ++i)
        {
            listUIManager.RemoveListRoomInfo(listUIManager.rooms[0]);
        }
        for (int i = 0, count = listUIManager.members.Count; i < count; ++i)
        {
            listUIManager.RemoveListMemberInfo(listUIManager.members[0]);
        }
    }
    public void Room_Back()
    {
        SetUI(UIState.Choise);
    }
    public void Room_GameStart()
    {

    }
    public void Room_GameReady()
    {

    }

    void SetUI(UIState state_)
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

    NetState CheckNetState(ref string buffer_)
    {
        var state = buffer_.Substring(0, buffer_.IndexOf("_"));
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        
        try
        {
            return (NetState)Enum.Parse(typeof(NetState), state);
        }
        catch (ArgumentException)
        {
            return NetState.Error;
        }
    }

    string Get_OpenData(string name_, bool passward_, string option_)
    {
        return (int)NetState.Room + "_" + name_ + "_" + passward_ + "_" + option_;
    }
    string[] Catch_OpenData(IPEndPoint endP_, string buffer_)
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
    string Get_RequireData(string name_, string passward_)
    {
        return (int)NetState.ConnectRequire + "_" + name_ + "_" + passward_;
    }
    string[] Catch_RequireData(IPEndPoint endP_, string buffer_)
    {
        var data = new string[3];
        data[0] = endP_.Address.ToString();
        data[1] = buffer_.Substring(0, buffer_.IndexOf("_"));//name
        data[2] = buffer_.Substring(buffer_.IndexOf("_") + 1);//passward
        return data;
    }
    string Get_ResponseData(bool flag_)
    {
        return (int)NetState.ConnectResponse + "_" + flag_;
    }
    string[] Catch_ResponseData(IPEndPoint endP_, string buffer_)
    {
        var data = new string[2];
        data[0] = endP_.Address.ToString();
        data[1] = buffer_.Substring(buffer_.IndexOf("_") + 1);//flag
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

    #region Editor
#if UNITY_EDITOR
    [ContextMenu("Send")]
    void SendTest()
    {
        var endP = RoomManager.buildEndP;
        var buffer = Encoding.UTF8.GetBytes(Get_OpenData("TestSendRoom", false, "hatune miku"));
        RoomManager.Udp?.SendAsync(buffer, buffer.Length, endP);
    }
    [ContextMenu("SendLock")]
    void SendTestLocked()
    {
        var endP = RoomManager.buildEndP;
        var buffer = Encoding.UTF8.GetBytes(Get_OpenData("TestSendRoom", true, "hatune miku"));
        RoomManager.Udp?.SendAsync(buffer, buffer.Length, endP);
    }
    [ContextMenu("SendConnect")]
    void SendTestConnect()
    {
        var endP = new IPEndPoint(RoomManager.GetLocalIPAddress(), RoomManager.Port);
        var buffer = Encoding.UTF8.GetBytes(Get_RequireData("Test", "3939"));
        RoomManager.Udp?.SendAsync(buffer, buffer.Length, endP);
    }
    [ContextMenu("Receive")]
    void ReceiveTest()
    {
        try
        {
            var endP = RoomManager.searchEndP;
            var buffer = RoomManager.Udp.Receive(ref endP);
            var data = Encoding.UTF8.GetString(buffer);
            Debug.Log(data);
        }
        catch (SocketException)
        {
            Debug.Log("No catch");
        }
        catch (ObjectDisposedException)
        {
#if UNITY_EDITOR
            Debug.Log("Socket Close Because Udp is Disposed");
#endif
        }
    }
#endif
    #endregion
}