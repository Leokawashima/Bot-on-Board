using System.Net;

// 制作者 日本電子専門学校　ゲーム制作科　22CI0212　川島

public struct UDPMessage_RoomData
{
    public IPAddress address;
    public string name;
    public bool passwardFlag;
    public string option;
    public int userMax;
    public int userCnt;
}
public struct UDPMessage_ConnectRequestData
{
    public IPAddress address;
    public string name;
    public string passward;
}
public struct MemberData
{
    public IPAddress address;
    public string name;
    public bool ready;
}
public struct UDPMessage_HostSubscribeData
{
    public IPAddress address;
    public MemberData[] members;
}
public struct UDPMessage_ClientSubscribeData
{
    public IPAddress address;
    public string name;
}
public struct UDPMessage_FlagData
{
    public IPAddress address;
    public bool flag;
}
public struct HostResponseData
{
    public IPEndPoint connectEndP;
    public bool connectFlag;
    public int connectCnt;
    public void AddCnt()
    {
        connectCnt++;
    }
}