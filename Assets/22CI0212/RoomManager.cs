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
    public enum State { Error = -1, Close, Host, Client, ConnectRequire, ConnectResponse }
    public static State state = State.Close;

    public static ushort Port { get; private set; } = 3939;
    public static int SendDelay { get; private set; } = 1000;
    public static int ReceiveDelay { get; private set; } = 1000;
    public static int SendTimeOut { get; private set; } = 100;
    public static int ReceiveTimeOut { get; private set; } = 100;

    public static UdpClient Udp { get; private set; }

    public static Action<IPEndPoint, string> CallBackClient;
    public static Action<IPEndPoint, string> CallBackResponse;
    public static Action<IPEndPoint, string> CallBackConnectResponse;

    public static IPEndPoint buildEndP { get { return new IPEndPoint(IPAddress.Broadcast, Port); } }
    public static IPEndPoint searchEndP { get { return new IPEndPoint(IPAddress.Any, Port); } }
    static IPAddress localIPAddress;
    #endregion

    #region Room
    public static async void Host(string buffer_)
    {
        state = State.Host;
        Udp = CreateUdp();

        var endP = buildEndP;
        var buffer = Encoding.UTF8.GetBytes(buffer_);

        while(state == State.Host)
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
    }
    public static async void Response()
    {
        while (state == State.Host)
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
    }
    public static async void Client()
    {
        state = State.Client;
        Udp = CreateUdp();

        while(state == State.Client)
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
    }
    public static async void ConnectRequire(string buffer_, IPAddress address_)
    {
        state = State.ConnectRequire;
        Udp = CreateUdp();

        var endP = new IPEndPoint(address_, Port);
        var buffer = Encoding.UTF8.GetBytes(buffer_);

        while (state == State.ConnectRequire)
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
    }
    public static async void ConnectResponse(IPAddress address_)
    {
        while (state == State.ConnectRequire)
        {
            try
            {
                var endP = new IPEndPoint(address_, Port);
                var buffer = Udp.Receive(ref endP);
                var data = Encoding.UTF8.GetString(buffer);
                CallBackConnectResponse?.Invoke(endP, data);
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
    }
    public static async void HostMessage(string buffer_, IPAddress address_)
    {
        int count = 0;
        var endP = new IPEndPoint(address_, Port);
        var buffer = Encoding.UTF8.GetBytes(buffer_);

        while (state == State.Host)
        {
            try
            {
                await Udp.SendAsync(buffer, buffer.Length, endP);
                if(count++ >= 20)
                {
#if UNITY_EDITOR
                    Debug.Log("Sended 20 times");
#endif
                    break;
                }
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
    }
    public static void Close()
    {
        state = State.Close;
        Udp?.Dispose();
        Udp = null;
    }
    public static void Clean()
    {
        Close();
        CallBackClient = null;
        CallBackResponse = null;
        CallBackConnectResponse = null;
        localIPAddress = null;
    }
    #endregion

    #region Func
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
        if (localIPAddress == null)
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return localIPAddress = ip;
                }
            }
            localIPAddress = null;
        }
        return localIPAddress;
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