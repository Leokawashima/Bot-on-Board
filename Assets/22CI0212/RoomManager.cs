using UnityEngine;
using System.Net;
using System;
using System.Text;
using System.Threading.Tasks;

public class RoomManager : MonoBehaviour
{
    [SerializeField] RoomUIManager roomUI;
    [SerializeField] RoomLogManager roomLog;
    [SerializeField] RoomListManager roomList;
    [SerializeField] RoomMakeManager roomMake;
    [SerializeField] RoomConnectManager roomConnect;

    RoomData MyRoom;

    enum MessageState { Error = -1, R_Open, R_Request, R_Response, H_Subscribe, H_GameStart, C_Subscribe, C_GameReady }
    public enum HostState { Close, Host, GameReady }
    public HostState hostState = HostState.Close;
    public enum ClientState { Close, Search, ConnectRequest, Subscribe, Ready, Quit }
    public ClientState clientState = ClientState.Close;

    void OnDestroy()
    {
        RoomUDP.Clean();
    }

    public async void Host()
    {
        MyRoom = new RoomData()
        {
            address = RoomUDP.GetLocalIPAddress(),
            name = roomMake.getName,
            option = roomMake.getOption,
            passward = roomMake.getPasswardIsOn,
            userMax = roomMake.getUserMax,
            userCnt = 1
        };

        var host = new MemberData()
        {
            address = RoomUDP.GetLocalIPAddress(),
            name = "Host(Me)",
            ready = false
        };
        roomList.Add_MemberInfo(host);

        hostState = HostState.Host;
        RoomUDP.CreateUdp();

        var buffer = Encoding.UTF8.GetBytes(Convert_RoomData());

        while(hostState == HostState.Host)
        {
            if(MyRoom.userMax > MyRoom.userCnt)
            {
                if(await RoomUDP.Send(buffer, RoomUDP.buildEndP) == false)
                {
                    hostState = HostState.Close;
                }
            }

            if(MyRoom.userCnt > 1)
            {
                var subscribeBuffer = Encoding.UTF8.GetBytes(Convert_HostScbscribeData());
                foreach(var member in roomList.members)
                {
                    if(await RoomUDP.Send(subscribeBuffer, member.memberEndP) == false)
                    {
                        hostState = HostState.Close;
                        break;
                    }
                }
            }

            if(RoomUDP.Receive(HostReveive) == false)
            {
                hostState = HostState.Close;
            }

            await Task.Delay(RoomUDP.HostDelay);
        }

        hostState = HostState.GameReady;

        while(hostState == HostState.GameReady)
        {
            //開始するぞメッセージを送る　帰ってくるように待つ

            if(RoomUDP.Receive(HostReveive) == false)
            {
                hostState = HostState.Close;
            }

            await Task.Delay(RoomUDP.HostDelay);
        }

        //接続開始
    }
    void HostReveive(IPEndPoint endP_, string buffer_)
    {
        switch(CheckMessageState(ref buffer_))
        {
            case MessageState.R_Request:
                {
                    var data = Get_ConectRequestData(endP_, buffer_);
                    var flag = !roomMake.getPasswardIsOn && data.passward == roomMake.getPassward;
                    if(flag)
                    {
                        roomList.Add_MemberInfo(endP_.Address, data);
                        roomLog.LogPush(data.name + " joined");
                    }

                    //メッセージを接続要求者に返す
                }
                break;
            case MessageState.C_Subscribe:
                {
                    //接続者からの定期メッセージを処理する
                }
                break;
            case MessageState.C_GameReady:
                {
                    //ゲームスタートを押してこっちも準備ができたと知らせるメッセージを処理する
                }
                break;
        }
    }
    public async void Client()
    {
        clientState = ClientState.Search;
        RoomUDP.CreateUdp();

        while(clientState == ClientState.Search)
        {
            if(RoomUDP.Receive(ClientReceive) == false)
            {
                clientState = ClientState.Close;
            }

            await Task.Delay(RoomUDP.ClientDelay);
        }

        var endP = new IPEndPoint(roomList.selectRoom.roomData.address, RoomUDP.Port);
        var connectBuffer = Encoding.UTF8.GetBytes(Convert_ConectRequestData());

        while(clientState == ClientState.ConnectRequest)
        {
            if(await RoomUDP.Send(connectBuffer, endP) == false)
            {
                clientState = ClientState.Close;
            }

            if(RoomUDP.Receive(ClientReceive) == false)
            {
                clientState = ClientState.Close;
            }

            await Task.Delay(RoomUDP.ClientDelay);
        }

        var subscriveBuffer = Encoding.UTF8.GetBytes(Convert_ClientSubscribeData());

        while(clientState == ClientState.Subscribe)
        {
            if(await RoomUDP.Send(subscriveBuffer, endP) == false)
            {
                clientState = ClientState.Close;
            }

            if(RoomUDP.Receive(ClientReceive) == false)
            {
                clientState = ClientState.Close;
            }

            await Task.Delay(RoomUDP.ClientDelay);
        }
    }
    void ClientReceive(IPEndPoint endP_, string buffer_)
    {
        switch(CheckMessageState(ref buffer_))
        {
            case MessageState.R_Open:
                {
                    var data = Get_RoomData(endP_, buffer_);
                    if(endP_.Address != RoomUDP.GetLocalIPAddress())
                        roomList.Add_RoomInfo(endP_.Address, data);
                }
                break;
            case MessageState.R_Response:
                {
                    var data = Get_FlagData(endP_, buffer_);
                    if(data.flag)
                    {
                        roomLog.LogPush("Connected");
                        roomUI.SetUI(RoomUIManager.UIState.Client);
                    }
                    else
                    {
                        Client();
                        roomUI.SetUI(RoomUIManager.UIState.Client);
                    }
                }
                break;
            case MessageState.H_Subscribe:
                {
                    var data = Get_HostSubscribeData(endP_, buffer_);
                    roomList.RemoveAll_MemberInfo();
                    foreach(var member in data.members)
                        roomList.Add_MemberInfo(member);
                }
                break;
            case MessageState.H_GameStart:
                {
                    //実際にゲーム開始を伝えるメッセージを処理する
                }
                break;
        }
    }
    public void Close()
    {
        hostState = HostState.Close;
        clientState = ClientState.Close;
        RoomUDP.Close();
    }

    #region Send/Receive Converter
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

    string Convert_RoomData()
    {
        return (int)MessageState.R_Open + "_" + MyRoom.name + "_" + MyRoom.option.Length + "_" + MyRoom.option + "_" + MyRoom.passward + "_" + MyRoom.userMax + "_" + MyRoom.userCnt;
    }
    UDPMessage_RoomData Get_RoomData(IPEndPoint endP_, string buffer_)
    {
        //Matching等を使いたかったがOptionの文字まで切り出す可能性がある為SubStringで切り出し
        var data = new UDPMessage_RoomData();
        data.address = endP_.Address;
        data.name = buffer_.Substring(0, buffer_.IndexOf("_"));
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        var length = int.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        data.option = buffer_.Substring(0, length);
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        data.passwardFlag = bool.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        data.userMax = int.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
        data.userCnt = int.Parse(buffer_.Substring(buffer_.IndexOf("_") + 1));
        return data;
    }
    string Convert_ConectRequestData()
    {
        return (int)MessageState.R_Request + "_" + roomUI.nameText.text + "_" + roomConnect.getPaswardText.text;
    }
    UDPMessage_ConnectRequestData Get_ConectRequestData(IPEndPoint endP_, string buffer_)
    {
        var data = new UDPMessage_ConnectRequestData();
        data.address = endP_.Address;
        data.name = buffer_.Substring(0, buffer_.IndexOf("_"));
        data.passward = buffer_.Substring(buffer_.IndexOf("_") + 1);
        return data;
    }
    string Convert_HostScbscribeData()
    {
        var str = (int)MessageState.H_Subscribe + "_" + roomList.members.Count + "_";
        foreach(var member in roomList.members)
            str += member.memberAddress + "_" + member.memberName.Length + "_" + member.memberName + "_" + member.memberReady + "_";
        return str;
    }
    UDPMessage_HostSubscribeData Get_HostSubscribeData(IPEndPoint endP_, string buffer_)
    {
        var data = new UDPMessage_HostSubscribeData();
        data.address = endP_.Address;
        data.members = new MemberData[int.Parse(buffer_.Substring(0, buffer_.IndexOf("_")))];
        for(int i = 0; i < data.members.Length; ++i)
        {
            data.members[i] = new MemberData();
            buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
            data.members[i].address = IPAddress.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
            buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
            var length = int.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
            buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
            data.members[i].name = buffer_.Substring(0, length);
            buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
            data.members[i].ready = bool.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
        }
        return data;
    }
    string Convert_ClientSubscribeData()
    {
        return (int)MessageState.C_Subscribe + "_" + roomUI.nameText.text;
    }
    UDPMessage_ClientSubscribeData Get_ClientSubscribeData(IPEndPoint endP_, string buffer_)
    {
        var data = new UDPMessage_ClientSubscribeData();
        data.address = endP_.Address;
        data.name = buffer_;
        return data;
    }
    string Convert_FlagData(bool flag_)
    {
        return (int)MessageState.R_Response + "_" + flag_;
    }
    UDPMessage_FlagData Get_FlagData(IPEndPoint endP_, string buffer_)
    {
        var data = new UDPMessage_FlagData();
        data.address = endP_.Address;
        data.flag = bool.Parse(buffer_.Substring(buffer_.IndexOf("_") + 1));
        return data;
    }
    #endregion
}