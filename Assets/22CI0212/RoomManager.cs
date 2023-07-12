using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

/// <summary>
/// Roomの接続処理を管理するクラス
/// </summary>
public static class RoomManager
{
    #region Field
    public static ushort Port { get; private set; } = 3939;
    public static int SendDelay { get; private set; } = 1000;
    public static int ReceiveDelay { get; private set; } = 1000;
    public static int SendTimeOut { get; private set; } = 100;
    public static int ReceiveTimeOut { get; private set; } = 100;

    public static UdpClient Udp { get; private set; }

    public static Action<IPEndPoint, string> CallBackClient;
    public static Action<IPEndPoint, string> CallBackResponse;

    public static IPEndPoint buildEndP { get { return new IPEndPoint(IPAddress.Broadcast, Port); } }
    public static IPEndPoint searchEndP { get { return new IPEndPoint(IPAddress.Any, Port); } }
    #endregion

    #region Room
    public static async void Host(string buffer_)
    {
        Udp = CreateUdp();

        var endP = buildEndP;
        var buffer = Encoding.UTF8.GetBytes(buffer_);

        while(Udp != null)
        {
            try
            {
                await Udp.SendAsync(buffer, buffer.Length, endP);

                await Task.Delay(SendDelay);
            }
            catch(ObjectDisposedException)
            {
#if UNITY_EDITOR
                Debug.Log("Socket Close Because Udp is Disposed");
#endif
                break;
            }
        }

        Close();
    }
    public static async void Response()
    {
        while (Udp != null)
        {
            try
            {
                var endP = searchEndP;
                var buffer = Udp.Receive(ref endP);
                var data = Encoding.UTF8.GetString(buffer);
                CallBackResponse?.Invoke(endP, data);
            }
            catch (SocketException)
            {
                await Task.Delay(ReceiveDelay);
            }
            catch (ObjectDisposedException)
            {
#if UNITY_EDITOR
                Debug.Log("Socket Close Because Udp is Disposed");
#endif
                break;
            }
        }

        Close();
    }
    public static async void Client()
    {
        Udp = CreateUdp();

        while(Udp != null)
        {
            try
            {
                var endP = searchEndP;
                var buffer = Udp.Receive(ref endP);
                var data = Encoding.UTF8.GetString(buffer);
                CallBackClient?.Invoke(endP, data);
            }
            catch(SocketException)
            {
                await Task.Delay(ReceiveDelay);
            }
            catch(ObjectDisposedException)
            {
#if UNITY_EDITOR
                Debug.Log("Socket Close Because Udp is Disposed");
#endif
                break;
            }
        }

        Close();
    }
    public static async void Send(string buffer_, IPAddress address_)
    {
        Udp = CreateUdp();

        var endP = new IPEndPoint(address_, Port);
        var buffer = Encoding.UTF8.GetBytes(buffer_);

        while (Udp != null)
        {
            try
            {
                await Udp.SendAsync(buffer, buffer.Length, endP);

                await Task.Delay(SendDelay);
            }
            catch (ObjectDisposedException)
            {
#if UNITY_EDITOR
                Debug.Log("Socket Close Because Udp is Disposed");
#endif
                break;
            }
        }

        Close();
    }
    public static void Close()
    {
        Udp?.Dispose();
    }
    public static void Clean()
    {
        Close();
        CallBackClient = null;
        CallBackResponse = null;
    }
    #endregion

    static UdpClient CreateUdp()
    {
        var udp = new UdpClient(Port);
        udp.EnableBroadcast = true;
        udp.Client.SendTimeout = SendTimeOut;
        udp.Client.ReceiveTimeout = ReceiveTimeOut;
        return udp;
    }
    public static IPAddress GetLocalIPAddress()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip;
            }
        }
        return null;
    }

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
            Udp?.Dispose();
        }
    }
#endif
    #endregion
}