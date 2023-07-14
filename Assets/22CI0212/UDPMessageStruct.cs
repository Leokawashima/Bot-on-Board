using System.Net;

public struct UDPMessage_RoomData
{
    public IPAddress address;
    public string name;
    public bool passwardFlag;
    public string option;
}
public struct UDPMessage_RequestData
{
    public IPAddress address;
    public string name;
    public string passward;
}
public struct UDPMessage_ResponseData
{
    public IPAddress address;
    public bool connectFlag;
}