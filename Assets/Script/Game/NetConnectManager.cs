using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

/// <summary>
/// NGO接続を管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class NetConnectManager : MonoBehaviour
{
    public void Host()
    {
        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if (transport is Unity.Netcode.Transports.UTP.UnityTransport unityTransport)
        {
            unityTransport.ConnectionData.Port = RoomUDP.Port;
            unityTransport.ConnectionData.ServerListenAddress = "0.0.0.0";
        }
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }
    public void Client()
    {
        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if(transport is Unity.Netcode.Transports.UTP.UnityTransport unityTransport)
        {
            unityTransport.SetConnectionData(RoomUDP.ConnectIPAddress, RoomUDP.Port);
        }
        NetworkManager.Singleton.StartClient();
    }

    void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // 追加の承認手順が必要な場合は、追加の手順が完了するまでこれを true に設定します
        // true から false に遷移すると、接続承認応答が処理されます。
        response.Pending = true;

        if(NetworkManager.Singleton.ConnectedClients.Count >= RoomUDP.ConnectUserMax)
        {
            response.Approved = false;//接続を許可しない
            response.Pending = false;
            return;
        }

        response.Approved = true;//接続を許可
        response.CreatePlayerObject = true;
        response.PlayerPrefabHash = null;

        //PlayerObjectをスポーンする位置(nullの場合Vector3.zero)
        var position = new Vector3(0, 5, 4);
        position.x = 4 + 2 * NetworkManager.Singleton.ConnectedClients.Count;
        response.Position = position;
        //PlayerObjectをスポーン時の回転 (nullの場合Quaternion.identity)
        response.Rotation = Quaternion.identity;

        response.Pending = false;
    }
}