using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using RoomUDPSystem;

/// <summary>
/// NGO接続を管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class NetConnectManager : SingletonMonoBehaviour<NetConnectManager>
{
    public event Action
        Event_PlayersConnected,
        Event_PlayersDisconnected;

    public void Host()
    {
        var _transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if (_transport is UnityTransport _unityTransport)
        {
            _unityTransport.ConnectionData.Port = RoomUDP.Port;
            _unityTransport.ConnectionData.ServerListenAddress = "0.0.0.0";
        }
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        AddCallBacks();
        NetworkManager.Singleton.StartHost();
    }
    public void Client()
    {
        var _transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if(_transport is UnityTransport _unityTransport)
        {
            // RoomUDP.ConnectionIPAddressは初期化しない場合127.0.0.1になる
            _unityTransport.SetConnectionData(RoomUDP.ConnectIPAddress, RoomUDP.Port);
        }
        AddCallBacks();
        NetworkManager.Singleton.StartClient();
    }
    private void AddCallBacks()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnConnectedCallBack;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisConnectedCallBack;
    }

    //接続承認要求のコールバック処理　参考 https://yuru-uni.com/2023/02/09/multiplay-tutorial3/
    void ApprovalCheck(NetworkManager.ConnectionApprovalRequest _, NetworkManager.ConnectionApprovalResponse response_)
    {
        response_.Pending = true;

        if(NetworkManager.Singleton.ConnectedClients.Count >= RoomUDP.ConnectUserMax)
        {
            response_.Approved = false;
            response_.Pending = false;
#if UNITY_EDITOR
            Debug.Log("最大人数を超過して接続するアクセス要求を破棄しました");
#endif
            return;
        }
        
        response_.Approved = true;
        response_.CreatePlayerObject = true;
        response_.PlayerPrefabHash = null;

        // 生成座標
        response_.Position = new Vector3(NetworkManager.Singleton.ConnectedClients.Count, 0, 0);

        response_.Pending = false;
    }

    void OnConnectedCallBack(ulong count_)
    {
        // 接続時に全員接続完了しているか確認してからの処理を呼ぶので?.Invokeは必要ない
        if ((int)++count_ == 2)
            Event_PlayersConnected();
    }

    void OnDisConnectedCallBack(ulong count_)
    {
        Event_PlayersDisconnected();
    }
}