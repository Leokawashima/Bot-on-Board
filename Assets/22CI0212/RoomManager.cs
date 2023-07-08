using System.Collections.Generic;
using System;
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
    [SerializeField] int roomReceiveDelay = 1000;
    [SerializeField] int roomTimeOut = 100;
    [Header("TextField")]
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] uint logMax = 20;
    List<string> logStr = new List<string>();

    IPAddress buildAddress = IPAddress.Broadcast;
    IPAddress searchAdderess = IPAddress.Any;

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
            await Task.Delay(roomTimeOut);
        }

        roomEndComplete = false;

        var client = new UdpClient(roomPort);
        client.EnableBroadcast = true;
        client.Client.SendTimeout = roomTimeOut;
        var endP = new IPEndPoint(buildAddress, roomPort);

        var buffer = Encoding.UTF8.GetBytes(GetLocalIPAddress());

        LogPush("Host Started");

        while(roomEndOrder == false)
        {
            try
            {
                await client.SendAsync(buffer, buffer.Length, endP);

                await Task.Delay(roomSendDelay);
            }
            catch(ObjectDisposedException)
            {
                LogPush("Error : Conection Disposed");
                roomEndOrder = true;
            }
        }

        client.Close();
        client.Dispose();

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

        roomEndComplete = false;

        var client = new UdpClient(roomPort);
        client.EnableBroadcast = true;
        client.Client.ReceiveTimeout = roomTimeOut;
        var endP = new IPEndPoint(searchAdderess, roomPort);

        LogPush("Client Started");

        while(roomEndOrder == false)
        {
            try
            {
                var buffer = client.Receive(ref endP);
                var data = Encoding.UTF8.GetString(buffer);
                roomEndOrder = true;
                LogPush("Get Host IP : " + data);
                LogPush("Client Completed");
            }
            catch(SocketException)
            {
                await Task.Delay(roomReceiveDelay);
            }
            catch(ObjectDisposedException)
            {
                LogPush("Error : Conection Disposed");
                roomEndOrder = true;
            }
        }

        client.Close();
        client.Dispose();

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
    void LogPush(string msg_)
    {
        if (logStr.Count == logMax)
        {
            logStr.RemoveAt(0);
        }
        logStr.Add(msg_);

        logText.text = null;
        foreach (var item in logStr)
        {
            logText.text += item;
            logText.text += "\n";
        }
    }
    #endregion
}