using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System;
using System.Text;
using System.Threading.Tasks;
using RoomUDPSystem;

// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class RoomManager : MonoBehaviour
{
    [SerializeField] RoomUIManager roomUI;
    [SerializeField] LogSystem roomLog;
    [SerializeField] RoomListManager roomList;
    [SerializeField] RoomMakeManager roomMake;
    [SerializeField] RoomConnectionManager roomConnect;

    RoomData m_hostRoomData;
    List<HostResponseData> connections = new();

    enum MessageState
    {
        Error = -1, R_Open, R_Request, R_Response, H_Subscribe, H_GameStart, C_Subscribe, C_GameReady
    }
    public enum HostState
    {
        Close, Host, GameReady
    }
    public HostState hostState = HostState.Close;
    public enum ClientState
    {
        Close, Search, ConnectRequest, Subscribe, Ready, Quit
    }
    public ClientState clientState = ClientState.Close;

    void OnDestroy()
    {
        RoomUDP.Clean();
    }

    public async void Host()
    {
        m_hostRoomData = new RoomData(RoomUDP.GetLocalIPAddress(), roomMake.Name, roomMake.Option, roomMake.HasPassward, roomMake.UserMax, 1);
        RoomUDP.SetRoomUserMax(m_hostRoomData.UserMax);

        var host = new MemberData(RoomUDP.GetLocalIPAddress(), roomUI.nameText.text + "(Host)", false);
        roomList.AddMemberInfo(host);

        hostState = HostState.Host;
        RoomUDP.CreateUdp();


        while (hostState != HostState.Close)
        {
            switch(hostState)
            {
                case HostState.Host:
                    {
                        if(m_hostRoomData.UserMax > m_hostRoomData.UserCount)
                        {
                            var buffer = Encoding.UTF8.GetBytes(Convert_RoomData());
                            var endP = RoomUDP.buildEndP;

                            if(await RoomUDP.Send(buffer, endP) == false)
                            {
                                hostState = HostState.Close;
                            }
                        }

                        if(m_hostRoomData.UserCount > 1)
                        {
                            var buffer = Encoding.UTF8.GetBytes(Convert_HostScbscribeData());

                            foreach(var member in roomList.MemberList)
                            {
                                if(await RoomUDP.Send(buffer, member.EndPoint) == false)
                                {
                                    hostState = HostState.Close;
                                    break;
                                }
                            }
                        }

                        if (connections.Count > 0)
                        {
                            for(int i = 0; i < connections.Count; ++i)
                            {
                                if (connections[i].ConnectionCount < RoomUDP.MessageCount)
                                {
                                    var buffer = Encoding.UTF8.GetBytes(Convert_FlagData(connections[i].CanConnection));
                                    if (await RoomUDP.Send(buffer, connections[i].ConnectionEndPoint))
                                    {
                                        connections[i].AddCnt();
                                    }
                                    else
                                    {
                                        hostState = HostState.Close;
                                        break;
                                    }
                                }
                                else
                                {
                                    connections.Remove(connections[i]);
                                }
                            }
                        }

                        if(RoomUDP.Receive(HostReveive) == false)
                        {
                            hostState = HostState.Close;
                        }
                    }
                    break;
                case HostState.GameReady:
                    {
                        //開始するぞメッセージを送る　帰ってくるように待つ

                        if(RoomUDP.Receive(HostReveive) == false)
                        {
                            hostState = HostState.Close;
                        }
                    }
                    break;
            }

            await Task.Delay(RoomUDP.HostDelay);
        }
    }
    async void HostReveive(IPEndPoint endP_, string buffer_)
    {
        switch(CheckMessageState(ref buffer_))
        {
            case MessageState.R_Request:
                {
                    var data = Get_ConectRequestData(endP_, buffer_);
                    var flag = !roomMake.HasPassward && data.Passward == roomMake.Passward;
                    if(flag)
                    {
                        roomList.AddMemberInfo(endP_.Address, data);
                        m_hostRoomData.AddUserCount();
                        roomLog.LogPush(data.Name + " joined");
                    }

                    var response = new HostResponseData()
                    {
                        ConnectionEndPoint = endP_,
                        CanConnection= flag,
                        ConnectionCount = 0
                    };
                    connections.Add(response);
                    var buffer = Encoding.UTF8.GetBytes(Convert_FlagData(flag));
                    await RoomUDP.Send(buffer, endP_);
                }
                break;
            case MessageState.C_Subscribe:
                {
                    var data = Get_ClientSubscribeData(endP_, buffer_);
                    for (int i = 0; i < connections.Count; ++i)
                    {
                        if (connections[i].ConnectionEndPoint.Equals(endP_))
                        {
                            connections.Remove(connections[i]);
                        }
                    }
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

        while(clientState != ClientState.Close)
        {
            switch(clientState)
            {
                case ClientState.Search:
                    {
                        if(RoomUDP.Receive(ClientReceive_Search) == false)
                        {
                            clientState = ClientState.Close;
                        }
                    }
                    break;
                case ClientState.ConnectRequest:
                    {
                        var endP = new IPEndPoint(roomList.SelectInfoRoomData.RoomData.Address, RoomUDP.Port);
                        var buffer = Encoding.UTF8.GetBytes(Convert_ConectRequestData());

                        if(await RoomUDP.Send(buffer, endP) == false)
                        {
                            clientState = ClientState.Close;
                        }

                        if(RoomUDP.Receive(ClientReceive_ConnectRequest) == false)
                        {
                            clientState = ClientState.Close;
                        }
                    }
                    break;
                case ClientState.Subscribe:
                    {
                        var endP = new IPEndPoint(roomList.SelectInfoRoomData.RoomData.Address, RoomUDP.Port);
                        var buffer = Encoding.UTF8.GetBytes(Convert_ClientSubscribeData());

                        if(await RoomUDP.Send(buffer, endP) == false)
                        {
                            clientState = ClientState.Close;
                        }

                        if(RoomUDP.Receive(ClientReceive_Subscribe) == false)
                        {
                            clientState = ClientState.Close;
                        }
                    }
                    break;
            }

            await Task.Delay(RoomUDP.ClientDelay);
        }
    }
    void ClientReceive_Search(IPEndPoint endP_, string buffer_)
    {
        if (CheckMessageState(ref buffer_) == MessageState.R_Open)
        {
            var data = Get_RoomData(endP_, buffer_);
            if(endP_.Address != RoomUDP.GetLocalIPAddress())
                roomList.AddRoomInfo(endP_.Address, data);
        }
    }
    void ClientReceive_ConnectRequest(IPEndPoint endP_, string buffer_)
    {
        if(CheckMessageState(ref buffer_) == MessageState.R_Response)
        {
            var data = Get_FlagData(endP_, buffer_);
            if(data.CanConnection)
            {
                clientState = ClientState.Subscribe;
                roomLog.LogPush("Connect Room");

                roomList.ClearRoomInfo();
            }
            else
            {
                clientState = ClientState.Search;
                roomLog.LogPush("Conecct Failed");
            }
            roomUI.SetUI(RoomUIManager.UIState.Client);
        } 
    }
    void ClientReceive_Subscribe(IPEndPoint endP_, string buffer_)
    {
        if (CheckMessageState(ref  buffer_) == MessageState.H_Subscribe)
        {
            var data = Get_HostSubscribeData(endP_, buffer_);
            roomList.ClearMemberInfo();
            foreach(var member in data.Members)
                roomList.AddMemberInfo(member);
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
            return (MessageState)System.Enum.Parse(typeof(MessageState), state);
        }
        catch(ArgumentException)
        {
            return MessageState.Error;
        }
    }

    string Convert_RoomData()
    {
        return (int)MessageState.R_Open + "_" + m_hostRoomData.Name + "_"
            + m_hostRoomData.Option.Length + "_" + m_hostRoomData.Option + "_"
            + m_hostRoomData.HasPassward + "_" + m_hostRoomData.UserMax + "_" + m_hostRoomData.UserCount;
    }
    RoomData Get_RoomData(IPEndPoint endP_, string buffer_)
    {
        // Matching等を使いたかったがOptionの文字に制限を設けていないので、
        // Optionの文字まで切り出す可能性がある為SubStringで切り出しを行っている
        var _address = endP_.Address;
        var _name = buffer_.Substring(0, buffer_.IndexOf("_"));
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        var _length = int.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        var _option = buffer_.Substring(0, _length);
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        var _passwardFlag = bool.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        var _userMax = int.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
        var _userCnt = int.Parse(buffer_.Substring(buffer_.IndexOf("_") + 1));

        return new RoomData(_address, _name, _option, _passwardFlag, _userMax, _userCnt);
    }
    string Convert_ConectRequestData()
    {
        return (int)MessageState.R_Request + "_" + roomUI.nameText.text + "_" + roomConnect.PasswardInputField.text;
    }
    ConnectionRequestData Get_ConectRequestData(IPEndPoint endP_, string buffer_)
    {
        var data = new ConnectionRequestData();
        data.Address = endP_.Address;
        data.Name = buffer_.Substring(0, buffer_.IndexOf("_"));
        data.Passward = int.Parse(buffer_.Substring(buffer_.IndexOf("_") + 1));
        return data;
    }
    string Convert_HostScbscribeData()
    {
        var str = (int)MessageState.H_Subscribe + "_" + roomList.MemberList.Count + "_";
        foreach(var member in roomList.MemberList)
        {
            var _memberData = member.MemberData;
            str += _memberData.Address + "_" + _memberData.Name.Length + "_" + _memberData.Name + "_" + _memberData.IsReady + "_";
        }
        return str;
    }
    HostSubscribeData Get_HostSubscribeData(IPEndPoint endP_, string buffer_)
    {
        var data = new HostSubscribeData
        {
            Address = endP_.Address,
            Members = new MemberData[int.Parse(buffer_.Substring(0, buffer_.IndexOf("_")))]
        };
        for(int i = 0; i < data.Members.Length; ++i)
        {
            buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
            var _address = IPAddress.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
            buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
            var length = int.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
            buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
            var _name = buffer_.Substring(0, length);
            buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
            var _isReady = bool.Parse(buffer_.Substring(0, buffer_.IndexOf("_")));
            data.Members[i] = new MemberData(_address, _name, _isReady);
        }
        return data;
    }
    string Convert_ClientSubscribeData()
    {
        return (int)MessageState.C_Subscribe + "_" + roomUI.nameText.text;
    }
    ClientSubscribeData Get_ClientSubscribeData(IPEndPoint endP_, string buffer_)
    {
        var data = new ClientSubscribeData()
        {
            Address = endP_.Address,
            Name = buffer_,
        };

        return data;
    }
    string Convert_FlagData(bool flag_)
    {
        return (int)MessageState.R_Response + "_" + flag_;
    }
    ConnectionResponseData Get_FlagData(IPEndPoint endP_, string buffer_)
    {
        var data = new ConnectionResponseData
        {
            Address = endP_.Address,
            CanConnection = bool.Parse(buffer_.Substring(buffer_.IndexOf("_") + 1))
        };
        return data;
    }
    #endregion
}