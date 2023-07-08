using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Roomの接続処理を賄うクラス
/// </summary>
public class RoomManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject selectUI;
    [SerializeField] GameObject hostUI;
    [SerializeField] GameObject clientUI;
    [SerializeField] GameObject listUI;
    [Header("RoomSetting")]
    [SerializeField] ushort roomPort = 3939;
    [SerializeField] int roomSendDelay = 1000;
    [SerializeField] int roomReceiveDelay = 1000;
    [SerializeField] int roomTimeOut = 100;
    [Header("TextField")]
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] uint logMax = 20;
    List<string> logStr = new List<string>();

    enum State { Non, Host, Client }
    State state;

    static UdpClient client;

    IPEndPoint buildEndP { get { return new IPEndPoint(IPAddress.Broadcast, roomPort); } }
    IPEndPoint searchEndP { get { return new IPEndPoint(IPAddress.Any, roomPort); } }

    void Start()
    {
        logText.text = string.Empty;
        logStr.Clear();

        selectUI.SetActive(true);
        hostUI.SetActive(false);
        clientUI.SetActive(false);
        listUI.SetActive(false);
    }

    public async void Room_Host()
    {
        selectUI.SetActive(false);
        hostUI.SetActive(true);
        listUI.SetActive(true);

        state = State.Host;

        client = MakeUdp(roomPort, roomTimeOut);
        var endP = buildEndP;

        var buffer = Encoding.UTF8.GetBytes(GetRoomData(GetLocalIPAddress(), "Test"));

        LogPush("Host Started");

        while(state == State.Host)
        {
            try
            {
                await client.SendAsync(buffer, buffer.Length, endP);

                await Task.Delay(roomSendDelay);
            }
            catch(ObjectDisposedException)
            {
                LogPush("Error : Conection Disposed");
                state = State.Non;
            }
        }

        client.Dispose();
        state = State.Non;
    }
    public async void Room_Client()
    {
        selectUI.SetActive(false);
        clientUI.SetActive(true);
        listUI.SetActive(true);

        state = State.Client;

        client = MakeUdp(roomPort, roomTimeOut);
        var endP = searchEndP;

        LogPush("Client Started");

        while(state == State.Client)
        {
            try
            {
                var buffer = client.Receive(ref endP);
                var data = Encoding.UTF8.GetString(buffer);
                var str = CatchRoomData(data);
                LogPush("Get Host IP : " + str);
            }
            catch(SocketException)
            {
                await Task.Delay(roomReceiveDelay);
            }
            catch(ObjectDisposedException)
            {
                LogPush("Error : Conection Disposed");
                state = State.Non;
            }
        }

        client.Dispose();
        state = State.Non;
    }
    public void Room_Quit()
    {
        client.Dispose();
        state = State.Non;

        logText.text = string.Empty;
        logStr.Clear();

        selectUI.SetActive(true);
        hostUI.SetActive(false);
        clientUI.SetActive(false);
        listUI.SetActive(false);
    }
    public void Room_GameStart()
    {

    }
    public void Room_GameReady()
    {

    }

    #region functions
    UdpClient MakeUdp(ushort port_, int timeOut_)
    {
        var client = new UdpClient(port_);
        client.EnableBroadcast = true;
        client.Client.SendTimeout = timeOut_;
        client.Client.ReceiveTimeout = timeOut_;
        return client;
    }
    string GetRoomData(string address_, string option_)
    {
        return address_ + "_" + option_;
    }
    string CatchRoomData(string buffer_)
    {
        var address = buffer_.Substring('_');
        return address;
    }
    string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach(var ip in host.AddressList)
        {
            if(ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return string.Empty;
    }
    void LogPush(string msg_)
    {
        if(logStr.Count == logMax)
        {
            logStr.RemoveAt(0);
        }
        logStr.Add(msg_);

        logText.text = null;
        foreach(var item in logStr)
        {
            logText.text += item;
            logText.text += "\n";
        }
    }
    #endregion

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void Initialize()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            client?.Dispose();
        }
    }
#endif
}