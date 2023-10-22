using System;
using System.Net;

// UDPメッセージの送受信する構造体を取りまとめたスクリプト
// 制作者 日本電子専門学校　ゲーム制作科　22CI0212　川島
namespace RoomUDPSystem
{
    public class RoomData
    {
        public IPAddress Address { get; private set; }
        public string Name { get; private set; }
        public string Option { get; private set; }
        public bool HasPassward { get; private set; }
        public int UserMax { get; private set; }
        public int UserCount { get; private set; }

        [Obsolete("引数無しコンストラクタは禁止です", true)]
        public RoomData() {}

        public RoomData(IPAddress address_, string name_, string option_, bool onPassward_, int userMax_, int userCount_)
        {
            Address = address_;
            Name = name_;
            Option = option_;
            HasPassward = onPassward_;
            UserMax = userMax_;
            UserCount = userCount_;
        }

        public void AddUserCount()
        {
            UserCount++;
        }
    }
    public struct ConnectionRequestData
    {
        public IPAddress Address;
        public string Name;
        public int Passward;
    }
    public struct ConnectionResponseData
    {
        public IPAddress Address;
        public bool CanConnection;
    }

    public class MemberData
    {
        public IPAddress Address { get; private set; }
        public string Name { get; private set; }
        public bool IsReady { get; private set; }

        [Obsolete("引数無しコンストラクタは禁止です", true)]
        public MemberData() { }

        public MemberData(IPAddress address_, string name_, bool isReady_)
        {
            Address = address_;
            Name = name_;
            IsReady = isReady_;
        }

        public void SetIsReady(bool isReady_)
        {
            IsReady = isReady_;
        }
    }

    public struct HostSubscribeData
    {
        public IPAddress Address;
        public MemberData[] Members;
    }
    public struct ClientSubscribeData
    {
        public IPAddress Address;
        public string Name;
    }
    public struct HostResponseData
    {
        public IPEndPoint ConnectionEndPoint;
        public bool CanConnection;
        public int ConnectionCount;
        public void AddCnt()
        {
            ConnectionCount++;
        }
    }
}