using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public static class NetConnectManager
{
    public static void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }
    public static void Client()
    {
        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if(transport is Unity.Netcode.Transports.UTP.UnityTransport unityTransport)
        {
            var ipAddress = RoomUDP.ConnectIPAddress;
            unityTransport.SetConnectionData(ipAddress, RoomUDP.Port);
        }
        NetworkManager.Singleton.StartClient();
    }

    static void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // 追加の承認手順が必要な場合は、追加の手順が完了するまでこれを true に設定します
        // true から false に遷移すると、接続承認応答が処理されます。
        response.Pending = true;

        if(NetworkManager.Singleton.ConnectedClients.Count >= RoomUDP.ConnectMaxUser)
        {
            response.Approved = false;//接続を許可しない
            response.Pending = false;
            return;
        }

        response.Approved = true;//接続を許可
        response.CreatePlayerObject = true;
        response.PlayerPrefabHash = null;

        //PlayerObjectをスポーンする位置(nullの場合Vector3.zero)
        var position = new Vector3(0, 1, -8);
        position.x = -2 + 2 * (NetworkManager.Singleton.ConnectedClients.Count % 3);
        response.Position = position;

        //PlayerObjectをスポーン時の回転 (nullの場合Quaternion.identity)
        response.Rotation = Quaternion.identity;

        response.Pending = false;
    }
}
