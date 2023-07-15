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
    enum MessageState {
        Error = -1, R_Open, R_Request, R_Response,
        H_Subscribe, H_GameReady, H_GameStart,
        C_Subscribe, C_Join, C_Leave, C_Ready, C_NotReady, C_GameReady }

    #endregion

    void Start()
    {
        logText.text = string.Empty;
        logStr.Clear();

        SetUI(UIState.Choise);

        listUIManager = listUI.GetComponent<ListUIManager>();
        listUIManager.Initialize();

        void host(IPEndPoint endP_, string buffer_)
        {
            switch(CheckMessageState(ref buffer_))
            {
                case MessageState.R_Request:
                    {
                        var data = Get_ConectRequestData(endP_, buffer_);
                        var flag = !makePasswardToggle.isOn && data.passward == makePasswardText.text;
                        if(flag)
                        {
                            listUIManager.Add_MemberInfo(endP_.Address, data);
                            LogPush(data.name + " joined");
                        }

                        var message = Convert_FlagData(MessageState.R_Response, flag);
                        RoomManager.HostMessage(message, endP_.Address);
                    }
                    break;
                case MessageState.C_Subscribe:
                    {
                        //接続者からの定期メッセージを処理する
                    }
                    break;
                case MessageState.C_Join:
                    {
                        //Roomに接続するメッセージを処理する
                    }
                    break;
                case MessageState.C_Leave:
                    {
                        //Roomから離れるメッセージを処理する
                    }
                    break;
                case MessageState.C_Ready:
                    {
                        //準備完了メッセージを処理する
                    }
                    break;
                case MessageState.C_NotReady:
                    {
                        //準備未完了メッセージを処理する
                    }
                    break;
                case MessageState.C_GameReady:
                    {
                        //ゲームスタートを押してこっちも準備ができたと知らせるメッセージを処理する
                    }
                    break;
            }
        }
        RoomManager.CallBackHost = host;

        void client(IPEndPoint endP_, string buffer_)
        {
            switch(CheckMessageState(ref buffer_))
            {
                case MessageState.R_Open:
                    {
                        var data = Get_RoomData(endP_, buffer_);
                        if(endP_.Address != RoomManager.GetLocalIPAddress())
                            listUIManager.Add_RoomInfo(endP_.Address, data);
                    }
                    break;
                case MessageState.R_Response:
                    {
                        var data = Get_FlagData(endP_, buffer_);
                        RoomManager.Close();
                        if(data.flag)
                        {
                            LogPush("Connected");
                            SetUI(UIState.Client);
                            for(int i = 0, count = listUIManager.rooms.Count; i < count; ++i)
                                listUIManager.Remove_RoomInfo(listUIManager.rooms[0]);

                            listUIManager.Add_MemberInfo(endP_.Address, "Host");
                            listUIManager.Add_MemberInfo(RoomManager.GetLocalIPAddress(), "me");
                        }
                        else
                        {
                            Room_Client();
                        }
                    }
                    break;
                case MessageState.H_Subscribe:
                    {
                        //Room定期メッセージを 処理する　
                    }
                    break;
                case MessageState.H_GameReady:
                    {
                        //ゲーム開始するがよろしいか？を伝えるメッセージを処理する
                    }
                    break;
                case MessageState.H_GameStart:
                    {
                        //実際にゲーム開始を伝えるメッセージを処理する
                    }
                    break;
            }
        }
        RoomManager.CallBackClient = client;
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

        var buffer = Convert_ConectRequestData("Name", makePasswardText.text);
        RoomManager.ConnectRequire(buffer, listUIManager.selectRoom.roomAddress);

        LogPush("Connect Require"); 
    }
    public void Room_BackRoom()
    {
        SetUI(UIState.BackRoom);
    }
    public void Room_Host()
    {
        SetUI(UIState.Host);

        var buffer = Convert_RoomData(makeNameText.text, makePasswardToggle.isOn, makeOptionText.text);
        RoomManager.Host(buffer);

        listUIManager.Add_MemberInfo(RoomManager.GetLocalIPAddress(), "Host(Me)");

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

        listUIManager.Clear();
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
                break;
            case UIState.MakeRoom:
                selectUI.SetActive(false);
                makeRoomUI.SetActive(true);
                break;
            case UIState.ConnectRoom:
                selectUI.SetActive(false);
                clientUI.SetActive(true);
                listUI.SetActive(true);
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
                connectUI.SetActive(false);
                selectUI.SetActive(false);
                clientUI.SetActive(true);
                listUI.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// データのヘッダーからメッセージステートを読み取る
    /// </summary>
    MessageState CheckMessageState(ref string buffer_)
    {
        var state = buffer_.Substring(0, buffer_.IndexOf("_"));
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);

        try
        {
            return (MessageState)Enum.Parse(typeof(MessageState), state);
        }
        catch(ArgumentException)
        {
            return MessageState.Error;
        }
    }

    #region Send/Receive Converter
    string Convert_RoomData(string name_, bool passward_, string option_)
    {
        return (int)MessageState.R_Open + "_" + name_ + "_" + passward_ + "_" + option_;
    }
    UDPMessage_RoomData Get_RoomData(IPEndPoint endP_, string buffer_)
    {
        //Matching等を使いたかったがOptionの文字まで切り出す可能性がある為SubStringで切り出し
        var data = new UDPMessage_RoomData();
        data.address = endP_.Address;
        data.name = buffer_.Substring(0, buffer_.IndexOf("_"));
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        data.passwardFlag = bool.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
        data.option = buffer_.Substring(buffer_.IndexOf("_") + 1);
        return data;
    }
    string Convert_ConectRequestData(string name_, string passward_)
    {
        return (int)MessageState.R_Request + "_" + name_ + "_" + passward_;
    }
    UDPMessage_ConnectRequestData Get_ConectRequestData(IPEndPoint endP_, string buffer_)
    {
        var data = new UDPMessage_ConnectRequestData();
        data.address = endP_.Address;
        data.name = buffer_.Substring(0, buffer_.IndexOf("_"));
        data.passward = buffer_.Substring(buffer_.IndexOf("_") + 1);
        return data;
    }

    string Convert_FlagData(MessageState message_, bool flag_)
    {
        return (int)message_ + "_" + flag_;
    }
    UDPMessage_FlagData Get_FlagData(IPEndPoint endP_, string buffer_)
    {
        var data = new UDPMessage_FlagData();
        data.address = endP_.Address;
        data.flag = bool.Parse(buffer_.Substring(buffer_.IndexOf("_") + 1));
        return data;
    }
    #endregion

    /// <summary>
    /// ログに行を追加する
    /// </summary>
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
}