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
    #region Valiables
    [Header("UI")]
    [SerializeField] GameObject selectUI;
    [SerializeField] GameObject makeRoomUI;
    [SerializeField] GameObject listUI;
    [SerializeField] GameObject hostUI;
    [SerializeField] GameObject clientUI;
    [Header("RoomSetting")]
    [SerializeField] ushort roomPort = 3939;
    [SerializeField] int roomSendDelay = 1000;
    [SerializeField] int roomReceiveDelay = 1000;
    [SerializeField] int roomTimeOut = 100;
    [Header("TextField")]
    [SerializeField] TextMeshProUGUI optionText;
    [SerializeField] TextMeshProUGUI passwardText;
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] uint logMax = 20;
    List<string> logStr = new List<string>();

    enum ConectionState { Non, Host, Client }
    ConectionState conectState;

    static UdpClient client;
    RoomList roomList;

    IPEndPoint buildEndP { get { return new IPEndPoint(IPAddress.Broadcast, roomPort); } }
    IPEndPoint searchEndP { get { return new IPEndPoint(IPAddress.Any, roomPort); } }
    #endregion

    #region UnityEvent
    void Start()
    {
        logText.text = string.Empty;
        logStr.Clear();

        selectUI.SetActive(true);
        makeRoomUI.SetActive(false);
        hostUI.SetActive(false);
        clientUI.SetActive(false);
        listUI.SetActive(false);

        roomList = listUI.GetComponent<RoomList>();
    }
    #endregion

    #region Room
    public void Room_MakeRoom()
    {
        selectUI.SetActive(false);
        makeRoomUI.SetActive(true);
    }
    public async void Room_Host()
    {
        selectUI.SetActive(false);
        hostUI.SetActive(true);
        listUI.SetActive(true);

        conectState = ConectionState.Host;

        client = MakeUdp(roomPort, roomTimeOut);
        var endP = buildEndP;

        var buffer = Encoding.UTF8.GetBytes(GetRoomData(GetLocalIPAddress(), passwardText.text, optionText.text));

        LogPush("Host Started");

        while(conectState == ConectionState.Host)
        {
            try
            {
                await client.SendAsync(buffer, buffer.Length, endP);

                await Task.Delay(roomSendDelay);
            }
            catch(ObjectDisposedException)
            {
                LogPush("Error : Conection Disposed");
                conectState = ConectionState.Non;
            }
        }

        client.Dispose();
        conectState = ConectionState.Non;
    }
    public async void Room_Client()
    {
        selectUI.SetActive(false);
        clientUI.SetActive(true);
        listUI.SetActive(true);

        conectState = ConectionState.Client;

        client = MakeUdp(roomPort, roomTimeOut);
        var endP = searchEndP;

        LogPush("Client Started");

        while(conectState == ConectionState.Client)
        {
            try
            {
                var buffer = client.Receive(ref endP);
                LogPush(endP.Address.ToString());
                var data = Encoding.UTF8.GetString(buffer);
                var info = CatchRoomData(data);
                roomList.SetListRoomInfo(info);
            }
            catch(SocketException)
            {
                await Task.Delay(roomReceiveDelay);
            }
            catch(ObjectDisposedException)
            {
                LogPush("Error : Conection Disposed");
                conectState = ConectionState.Non;
            }
        }

        client.Dispose();
        conectState = ConectionState.Non;
    }
    public void Room_Quit()
    {
        client.Dispose();
        conectState = ConectionState.Non;

        logText.text = string.Empty;
        logStr.Clear();

        selectUI.SetActive(true);
        makeRoomUI.SetActive(false);
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
    #endregion

    #region Function
    UdpClient MakeUdp(ushort port_, int timeOut_)
    {
        var client = new UdpClient(port_);
        client.EnableBroadcast = true;
        client.Client.SendTimeout = timeOut_;
        client.Client.ReceiveTimeout = timeOut_;
        return client;
    }
    string GetRoomData(string address_, string passward_, string option_)
    {
        return address_ + "_" + passward_ + "_" + option_;
    }
    string[] CatchRoomData(string buffer_)
    {
        //Matching等を使いたかったがOptionの文字まで切り出す可能性がある為SubStringで切り出し
        var data = new string[3];
        data[0] = buffer_.Substring(0, buffer_.IndexOf("_"));//address
        buffer_ = buffer_.Substring(buffer_.IndexOf("_") + 1);
        data[1] = buffer_.Substring(0, buffer_.IndexOf("_"));//passward
        data[2] = buffer_.Substring(buffer_.IndexOf("_") + 1);//option
        return data;
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

    #region Editor
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
    [ContextMenu("Test")]
    void SendTest()
    {
        var endP = buildEndP;
        var buffer = Encoding.UTF8.GetBytes(GetRoomData(GetLocalIPAddress(), "2525", "hatune miku"));
        for(int i = 0; i < 3; ++i)
            client.SendAsync(buffer, buffer.Length, endP);
    }
#endif
    #endregion
}