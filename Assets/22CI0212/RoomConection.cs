using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class RoomConection : MonoBehaviour
{
    #region valiables
    [SerializeField] TMP_InputField text;
    [SerializeField] uint maxUser = 4;
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

    #region NGOevent
    public void Server()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartServer();
        text.text = GetLocalIPAddress();
    }
    public void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
        text.text = GetLocalIPAddress();
    }
    public void Client()
    {
        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if(transport is Unity.Netcode.Transports.UTP.UnityTransport unityTransport)
        {
            // 接続先のIPアドレスとポートを指定
            var ipAddress = text.text;
            unityTransport.SetConnectionData(ipAddress, port);
        }
        NetworkManager.Singleton.StartClient();
    }
    #endregion

    void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // 追加の承認手順が必要な場合は、追加の手順が完了するまでこれを true に設定します
        // true から false に遷移すると、接続承認応答が処理されます。
        response.Pending = true;

        //最大人数をチェック(この場合は4人まで)
        if(NetworkManager.Singleton.ConnectedClients.Count >= maxUser)
        {
            response.Approved = false;//接続を許可しない
            response.Pending = false;
            return;
        }

        //ここからは接続成功クライアントに向けた処理
        response.Approved = true;//接続を許可

        //PlayerObjectを生成するかどうか
        response.CreatePlayerObject = true;

        //生成するPrefabハッシュ値。nullの場合NetworkManagerに登録したプレハブが使用される
        response.PlayerPrefabHash = null;

        //PlayerObjectをスポーンする位置(nullの場合Vector3.zero)
        var position = new Vector3(0, 1, -8);
        position.x = -2 + 2 * (NetworkManager.Singleton.ConnectedClients.Count % 3);
        response.Position = position;

        //PlayerObjectをスポーン時の回転 (nullの場合Quaternion.identity)
        response.Rotation = Quaternion.identity;

        response.Pending = false;
    }

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
                var buffer = client.Receive(ref endP);//この処理は受信完了まで処理が停止する
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
