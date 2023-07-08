using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

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
    [SerializeField] bool roomEndOrder = true;
    [SerializeField] bool roomEndComplete = true;
    [SerializeField] ushort roomPort = 3939;
    [SerializeField] int roomSendDelay = 1000;
    [SerializeField] int roomSendTimeOut = 100;
    [SerializeField] int roomReceiveDelay = 1000;
    [SerializeField] int roomReceiveTimeOut = 100;
    [Header("TextField")]
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] uint logMax = 20;
    List<string> logStr = new List<string>();

    UdpClient udpObject;

    IPAddress buildAddress = IPAddress.Broadcast;
    IPAddress searchAdderess = IPAddress.Any;

    void OnEnable()
    {
        udpObject = new UdpClient(roomPort);
        udpObject.EnableBroadcast = true;
        udpObject.Client.SendTimeout = roomSendTimeOut;
        udpObject.Client.ReceiveTimeout = roomReceiveTimeOut;
    }
    void OnDisable()
    {
        udpObject.Close();
        udpObject.Dispose();
    }

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
        roomEndOrder = false;

        selectUI.SetActive(false);
        hostUI.SetActive(true);
        listUI.SetActive(true);

        while(roomEndComplete == false)
        {
            LogPush("Wait for Conect Complete");
            await Task.Delay(500);
        }

        var endP = new IPEndPoint(buildAddress, roomPort);

        var buffer = Encoding.UTF8.GetBytes(GetLocalIPAddress());

        LogPush("Host Started");

        while(roomEndOrder == false)
        {
            try
            {
                await udpObject.SendAsync(buffer, buffer.Length, endP);

                await Task.Delay(roomSendDelay);
            }
            catch(System.ObjectDisposedException)
            {
                LogPush("Error : Conection Disposed");
                roomEndOrder = true;
            }
        }

        roomEndComplete = true;
    }
    public async void Room_Client()
    {
        roomEndOrder = false;

        selectUI.SetActive(false);
        clientUI.SetActive(true);
        listUI.SetActive(true);

        while(roomEndComplete == false)
        {
            LogPush("Wait for Conect Complete");
            await Task.Delay(500);
        }

        var endP = new IPEndPoint(searchAdderess, roomPort);

        LogPush("Client Started");

        while(roomEndOrder == false)
        {
            try
            {
                var buffer = udpObject.Receive(ref endP);
                var data = Encoding.UTF8.GetString(buffer);
                roomEndOrder = true;
                LogPush("Get Host IP : " + data);
            }
            catch (SocketException)
            {
                await Task.Delay(roomReceiveDelay);
            }
            catch (System.ObjectDisposedException)
            {
                LogPush("Error : Conection Disposed");
                roomEndOrder = true;
            }
        }

        roomEndComplete = true;
    }
    public void Room_Quit()
    {
        roomEndOrder = true;
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
    string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return string.Empty;
    }
    void LogPush(string msg)
    {
        if (logStr.Count == logMax)
        {
            logStr.RemoveAt(0);
        }
        logStr.Add(msg);

        logText.text = null;
        foreach (var item in logStr)
        {
            logText.text += item;
            logText.text += "\n";
        }
    }
    #endregion
}
