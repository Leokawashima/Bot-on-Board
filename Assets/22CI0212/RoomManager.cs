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
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public static class RoomManager
{
    #region Field
    public enum State { Error = -1, Close, Host, Client, ConnectRequire, ConnectResponse }
    public static State state = State.Close;

    public static ushort Port { get; private set; } = 3939;
    public static int HostDelay { get; private set; } = 1000;
    public static int ClientDelay { get; private set; } = 1000;
    public static int ConnectDelay { get; private set; } = 1000;

    public static int SendTimeOut { get; private set; } = 10;
    public static int ReceiveTimeOut { get; private set; } = 10;

    public static UdpClient Udp { get; private set; }

    public static Action<IPEndPoint, string> CallBack_ReceiveHost;
    public static Action<IPEndPoint, string> CallBack_ReceiveClient;
    public static Action<IPEndPoint, string> CallBack_ReceiveConnect;

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
            }
            catch(ObjectDisposedException)
            {
#if UNITY_EDITOR
                Debug.Log("Socket Close Because Udp is Disposed");
#endif
                break;
            }

            try
            {
                while(true)
                {
                    var remote = searchEndP;
                    var getbuffer = Udp.Receive(ref remote);
                    var data = Encoding.UTF8.GetString(getbuffer);
                    CallBack_ReceiveHost?.Invoke(remote, data);
                }
            }
            catch(SocketException)
            {
                await Task.Delay(HostDelay);
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

    public static async void Client()
    {
        state = State.Client;
        Udp = CreateUdp();

        while(state == State.Client)
        {
            try
            {
                while(true)
                {
                    var endP = searchEndP;
                    var buffer = Udp.Receive(ref endP);
                    var data = Encoding.UTF8.GetString(buffer);
                    CallBack_ReceiveClient?.Invoke(endP, data);
                }
            }
            catch(SocketException)
            {
                await Task.Delay(ClientDelay);
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

    public static async void Connect(string buffer_, IPAddress address_)
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
            }
            catch (ObjectDisposedException)
            {
#if UNITY_EDITOR
                Debug.Log("Socket Close Because Udp is Disposed");
#endif
                break;
            }

            try
            {
                while(true)
                {
                    var remote = new IPEndPoint(address_, Port);
                    var getbuffer = Udp.Receive(ref remote);
                    Debug.Log("get");
                    var data = Encoding.UTF8.GetString(getbuffer);
                    CallBack_ReceiveConnect?.Invoke(endP, data);
                }
            }
            catch(SocketException)
            {
                await Task.Delay(ConnectDelay);
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

    public static async void HostMessage(string buffer_, IPAddress address_)
    {
        int cnt = 0;
        var endP = new IPEndPoint(address_, Port);
        var buffer = Encoding.UTF8.GetBytes(buffer_);

        while (state == State.Host)
        {
            try
            {
                await Udp.SendAsync(buffer, buffer.Length, endP);
                if(cnt++ >= 20)
                {
                    Debug.Log("Host sended");
                    break;
                }
                
                await Task.Delay(1000);
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
        CallBack_ReceiveHost = null;
        CallBack_ReceiveClient = null;
        CallBack_ReceiveConnect = null;
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