using System.Net;

public struct UDPMessage_RoomData
{
    public IPAddress address;
    public string name;
    public bool passwardFlag;
    public string option;
}
public struct UDPMessage_ConnectRequestData
{
    public IPAddress address;
    public string name;
    public string passward;
}
public struct UDPMessage_FlagData
{
    public IPAddress address;
    public bool flag;
}