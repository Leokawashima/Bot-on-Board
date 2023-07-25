using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

/// <summary>
/// RoomのUDP接続処理を管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public static class RoomUDP
{
    #region Field
    public static ushort Port { get; private set; } = 3939;
    public static int MessageCount { get; private set; } = 10;
    public static int HostDelay { get; private set; } = 1000;
    public static int ClientDelay { get; private set; } = 1000;
    public static int SendTimeOut { get; private set; } = 100;
    public static int ReceiveTimeOut { get; private set; } = 100;
    public static string ConnectIPAddress { get; private set; } = "127.0.0.1";
    public static int ConnectUserMax { get; private set; } = 2;
    public static int connectIndex { get; private set; }

    public static UdpClient Udp { get; private set; }

    public static IPEndPoint buildEndP { get { return new IPEndPoint(IPAddress.Broadcast, Port); } }
    public static IPEndPoint searchEndP { get { return new IPEndPoint(IPAddress.Any, Port); } }
    static IPAddress localIPAddress;
    #endregion

    #region Func
    public static void CreateUdp()
    {
        Udp = new UdpClient(Port);
        Udp.EnableBroadcast = true;
        Udp.Client.SendTimeout = SendTimeOut;
        Udp.Client.ReceiveTimeout = ReceiveTimeOut;
    }
    public static void SetRoomIPAddress(IPAddress address_)
    {
        ConnectIPAddress = address_.ToString();
    }
    public static void SetRoomUserMax(int userMax_)
    {
        ConnectUserMax = userMax_;
    }
    public static IPAddress GetLocalIPAddress()
    {
        if(localIPAddress == null)
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach(IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return localIPAddress = ip;
                }
            }
            localIPAddress = null;
        }
        return localIPAddress;
    }
    public static async Task<bool> Send(byte[] buffer_, IPEndPoint endP_)
    {
        try
        {
            await Udp.SendAsync(buffer_, buffer_.Length, endP_);
            return true;
        }
        catch(ObjectDisposedException)
        {
#if UNITY_EDITOR
            Debug.Log("Socket Close Because Udp is Disposed");
#endif
            return false;
        }
        catch(NullReferenceException)
        {
#if UNITY_EDITOR
            Debug.Log("Socket Close Because Udp is Null");
#endif
            return false;
        }
    }
    public static bool Receive(Action<IPEndPoint, string> callback_)
    {
        try
        {
            while(true)
            {
                var endP = searchEndP;
                var buffer = Udp.Receive(ref endP);
                var data = Encoding.UTF8.GetString(buffer);
                callback_?.Invoke(endP, data);
            }
        }
        catch(SocketException)
        {
            return true;
        }
        catch(ObjectDisposedException)
        {
#if UNITY_EDITOR
            Debug.Log("Socket Close Because Udp is Disposed");
#endif
            return false;
        }
        catch(NullReferenceException)
        {
#if UNITY_EDITOR
            Debug.Log("Socket Close Because Udp is Null");
#endif
            return false;
        }
    }
    public static void Close()
    {
        Udp?.Dispose();
        Udp = null;
    }
    public static void Clean()
    {
        Close();
        localIPAddress = null;
    }
    #endregion

    #region Editor
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void Initialize()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    static void OnPlayModeStateChanged(PlayModeStateChange state_)
    {
        if(state_ == PlayModeStateChange.ExitingPlayMode)
        {
            //通信中にEditorの再生が停止した場合終了されないので終了するための処理
            Clean();
        }
    }
#endif
    #endregion
}
