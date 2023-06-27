using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class NetTest : MonoBehaviour
{
    [SerializeField] TMP_InputField text;
    [SerializeField] uint maxUser = 4;
    [SerializeField] ushort port = 7777;
    [Header("Build")]
    [SerializeField] bool roomBuildContinueFlag = false;
    [SerializeField] IPAddress buildAddress = IPAddress.Broadcast;//new IPAddress(new byte[4] { 127, 0, 0, 1})
    [SerializeField] int roomSendDelay = 5000;
    [SerializeField] int roomSendTimeOut = 1000;
    [Header("Search")]
    [SerializeField] bool roomSearchContinueFlag = false;
    [SerializeField] IPAddress SearchAdderess = IPAddress.Any;//new IPAddress(new byte[4] { 127, 0, 0, 1 })
    [SerializeField] int roomCatchDelay = 5000;
    [SerializeField] int roomReceiveTimeOut = 500;
    [Header("テキストフィールド")]
    [SerializeField] TextMeshProUGUI catchText;
    [SerializeField] TextMeshProUGUI logText;
    
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

    //ローカルIP
    string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach(var ip in host.AddressList)
        {
            if(ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return string.Empty;
    }

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

    public async void StartRoom()
    {
        if (roomBuildContinueFlag || roomSearchContinueFlag) return;

        roomBuildContinueFlag = true;

        var client = new UdpClient(port);
        client.EnableBroadcast = true;
        client.Client.SendTimeout = roomSendTimeOut;
        var endP = new IPEndPoint(buildAddress, port);
        client.Connect(endP);
        
        var buffer = Encoding.UTF8.GetBytes(GetLocalIPAddress());

        while (roomBuildContinueFlag)
        {
            logText.text = "send started...";
            await client.SendAsync(buffer, buffer.Length);

            logText.text = "send completed... waiting for delay...";
            await Task.Delay(roomSendDelay);
        }

        client.Close();
        client.Dispose();
        logText.text = "send stopped";
    }

    public async void GetRoom()
    {
        if (roomBuildContinueFlag || roomSearchContinueFlag) return;

        roomSearchContinueFlag = true;

        var client = new UdpClient(port);
        client.EnableBroadcast = true;
        client.Client.ReceiveTimeout = roomReceiveTimeOut;
        var endP = new IPEndPoint(SearchAdderess, port);

        while (roomSearchContinueFlag)
        {
            try
            {
                logText.text = "search started...";
                var buffer = client.Receive(ref endP);//この処理は受信完了まで処理が停止する
                var data = Encoding.UTF8.GetString(buffer);
                catchText.text = data;
                roomSearchContinueFlag = false;
                logText.text = "getting rooms";
            }
            catch (SocketException e)
            {   logText.text = e.Message;   }

            logText.text = "search completed... waiting for delay";
            await Task.Delay(roomCatchDelay);
        }

        client.Close();
        client.Dispose();
        logText.text = "search stopped";
    }

    public void StopRoom()
    {
        roomBuildContinueFlag = false;
        roomSearchContinueFlag = false;
    }
}
