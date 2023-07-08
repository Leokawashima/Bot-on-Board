using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

/// <summary>
/// Room�̐ڑ�������d���N���X
/// </summary>
public class RoomConection : MonoBehaviour
{
    #region valiables
    [SerializeField] TMP_InputField text;
    [SerializeField] ushort port = 7777;
    [Header("Build")]
    [SerializeField] bool roomBuildContinueFlag = false;
    [SerializeField] bool roomBuildEndFlag = true;
    [SerializeField] IPAddress buildAddress = IPAddress.Broadcast;
    [SerializeField] int roomSendDelay = 1000;
    [SerializeField] int roomSendTimeOut = 500;
    [Header("Search")]
    [SerializeField] bool roomSearchContinueFlag = false;
    [SerializeField] bool roomSearchEndFlag = true;
    [SerializeField] IPAddress SearchAdderess = IPAddress.Any;
    [SerializeField] int roomCatchDelay = 1000;
    [SerializeField] int roomReceiveTimeOut = 500;
    [Header("TextField")]
    [SerializeField] TextMeshProUGUI catchText;
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] uint logSize = 20;
    List<string> logStr = new List<string>();
    #endregion

    #region RoomEvent
    public async void StartRoom()
    {
        if (!roomBuildEndFlag || !roomSearchEndFlag) return;

        roomBuildContinueFlag = true;
        roomBuildEndFlag = false;

        var client = new UdpClient(port);
        client.EnableBroadcast = true;
        client.Client.SendTimeout = roomSendTimeOut;
        var endP = new IPEndPoint(buildAddress, port);
        client.Connect(endP);
        
        var buffer = Encoding.UTF8.GetBytes(GetLocalIPAddress());

        while (roomBuildContinueFlag)
        {
            LogPush("send started...");
            await client.SendAsync(buffer, buffer.Length);

            LogPush("send completed... waiting for delay...");
            await Task.Delay(roomSendDelay);
        }

        client.Close();
        client.Dispose();
        roomBuildEndFlag = true;
        LogPush("send stopped");
    }
    public async void GetRoom()
    {
        if (!roomBuildEndFlag || !roomSearchEndFlag) return;

        roomSearchContinueFlag = true;
        roomSearchEndFlag = false;

        var client = new UdpClient(port);
        client.EnableBroadcast = true;
        client.Client.ReceiveTimeout = roomReceiveTimeOut;
        var endP = new IPEndPoint(SearchAdderess, port);

        while (roomSearchContinueFlag)
        {
            try
            {
                LogPush("search started...");
                var buffer = client.Receive(ref endP);//���̏����͎�M�����܂ŏ�������~����
                var data = Encoding.UTF8.GetString(buffer);
                catchText.text = data;
                roomSearchContinueFlag = false;
                LogPush("getting rooms");
            }
            catch (SocketException)
            {
                LogPush("search missing... waiting for delay");
                await Task.Delay(roomCatchDelay);
            }

            
        }

        client.Close();
        client.Dispose();
        roomSearchEndFlag = true;
        LogPush("search stopped");
    }
    public void StopRoom()
    {
        LogPush("require room stop");
        roomBuildContinueFlag = false;
        roomSearchContinueFlag = false;
    }
    #endregion

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
        if (logStr.Count == logSize)
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
