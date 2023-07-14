using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
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
        CreateUdp();

        var endP = buildEndP;
        var buffer = Encoding.UTF8.GetBytes(buffer_);

        Receive(State.Host, CallBackResponse);
        while(state == State.Host)
        {
            if(await Send(buffer, endP) == false)
            {
                break;
            }
        }
    }

    public static void Client()
    {
        state = State.Client;
        CreateUdp();

        Receive(State.Client, CallBackClient);
    }

    public static async void ConnectRequire(string buffer_, IPAddress address_)
    {
        state = State.ConnectRequire;
        CreateUdp();

        var endP = new IPEndPoint(address_, Port);
        var buffer = Encoding.UTF8.GetBytes(buffer_);

        Receive(State.ConnectRequire, CallBackConnectResponse);
        while(state == State.ConnectRequire)
        {
            if(await Send(buffer, endP) == false)
            {
                break;
            }
        }
    }

    public static async void HostMessage(string buffer_, IPAddress address_)
    {
        int count = 0;
        var endP = buildEndP;
        var buffer = Encoding.UTF8.GetBytes(buffer_);

        while (state == State.Host)
        {
            if(await Send(buffer, endP))
            {
                if(count++ >= 20)
                    break;
            }
            else
            {
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
    static void CreateUdp()
    {
        Udp = new UdpClient(Port);
        Udp.EnableBroadcast = true;
        Udp.Client.SendTimeout = SendTimeOut;
        Udp.Client.ReceiveTimeout = ReceiveTimeOut;
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
    static async Task<bool> Send(byte[] buffer_, IPEndPoint endP_)
    {
        try
        {
            await Udp.SendAsync(buffer_, buffer_.Length, endP_);

            await Task.Delay(SendDelay);
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
    static async void Receive(State state_, Action<IPEndPoint, string> callback_)
    {
        #region Receive
        while(state == state_)
        {
            try
            {
                var endP = searchEndP;
                var buffer = Udp.Receive(ref endP);
                var data = Encoding.UTF8.GetString(buffer);
                callback_?.Invoke(endP, data);
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
            catch(NullReferenceException)
            {
#if UNITY_EDITOR
                Debug.Log("Socket Close Because Udp is Null");
#endif
                break;
            }
        }
        #endregion

        #region ReceiveAsync
        /*
        //ReceiveAsync方式
        //文字列に変換しないと自身のアドレスとの比較が上手くいかない(IPAddressが参照型だから比較できない？)
        //文字を受け取ったら勝手に仕事してまた探すのでメッセージが多い時も少ない時も使い勝手が良いが気に入らないので使っていない
        //ReceiveAsyncはキャンセルトークンも渡せないようなので余計にヘイトが高い
        //使いたい場合はコメント外してね
        try
        {
            while(state == state_)
            {
                var result = await Udp.ReceiveAsync();
                var data = Encoding.UTF8.GetString(result.Buffer);
                callback_?.Invoke(result.RemoteEndPoint, data);
            }
        }
        catch (ObjectDisposedException)
        {
            //なにがし
        }
        catch(NullReferenceException)
        {
            //それがし
        }
        */
        #endregion
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