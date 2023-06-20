using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class NetTest : MonoBehaviour
{
    [SerializeField] TMP_InputField text;
    [SerializeField] uint maxUser = 4;
    [SerializeField] ushort port = 7777;
    [SerializeField] bool roomBuildContinueFlag = false;
    [SerializeField] int roomSendDelay = 5000;
    [SerializeField] bool roomSearchContinueFlag = false;
    [SerializeField] int roomCatchDelay = 5000;
    [SerializeField] int roomReceiveTimeOut = 100;
    [SerializeField] TextMeshProUGUI catchText;
    UdpClient client;
    
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
            // �ڑ����IP�A�h���X�ƃ|�[�g���w��
            var ipAddress = text.text;
            unityTransport.SetConnectionData(ipAddress, port);
        }
        NetworkManager.Singleton.StartClient();
    }

    //���[�J��IP
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
        // �ǉ��̏��F�菇���K�v�ȏꍇ�́A�ǉ��̎菇����������܂ł���� true �ɐݒ肵�܂�
        // true ���� false �ɑJ�ڂ���ƁA�ڑ����F��������������܂��B
        response.Pending = true;

        //�ő�l�����`�F�b�N(���̏ꍇ��4�l�܂�)
        if(NetworkManager.Singleton.ConnectedClients.Count >= maxUser)
        {
            response.Approved = false;//�ڑ��������Ȃ�
            response.Pending = false;
            return;
        }

        //��������͐ڑ������N���C�A���g�Ɍ���������
        response.Approved = true;//�ڑ�������

        //PlayerObject�𐶐����邩�ǂ���
        response.CreatePlayerObject = true;

        //��������Prefab�n�b�V���l�Bnull�̏ꍇNetworkManager�ɓo�^�����v���n�u���g�p�����
        response.PlayerPrefabHash = null;

        //PlayerObject���X�|�[������ʒu(null�̏ꍇVector3.zero)
        var position = new Vector3(0, 1, -8);
        position.x = -2 + 2 * (NetworkManager.Singleton.ConnectedClients.Count % 3);
        response.Position = position;

        //PlayerObject���X�|�[�����̉�] (null�̏ꍇQuaternion.identity)
        response.Rotation = Quaternion.identity;

        response.Pending = false;
    }

    public async void StartRoom()
    {
        if (roomBuildContinueFlag || roomSearchContinueFlag) return;
        if (client != null) return;

        roomBuildContinueFlag = true;
        //var buffer = Encoding.UTF8.GetBytes(GetLocalIPAddress());

        var endP = new IPEndPoint(IPAddress.Broadcast, port);

        //�񓯊������ɂ��ă~���b�P�ʋ�؂�Ń��[�v���񂵂āA
        //�����Ԃ��Ƃ�BloadCast�A�h���X�Ƀ��[�����𑗂葱����
        while (roomBuildContinueFlag)
        {
            client = new UdpClient(port);
            client.EnableBroadcast = true;
            client.Connect(endP);
            //udpClient.Client.SendTimeout = 5000; //send�ɂ��^�C���A�E�g�@�\��t�^����
            //client.Send(buffer, buffer.Length);
            client.Close();

            await Task.Delay(roomSendDelay);
        }
    }

    public async void GetRoom()
    {
        if (roomBuildContinueFlag || roomSearchContinueFlag) return;
        if (client != null) return;

        roomSearchContinueFlag = true;

        var remote = new IPEndPoint(IPAddress.Any, port);

        while (roomSearchContinueFlag)
        {
            try
            {
                client = new UdpClient(port);
                client.Client.ReceiveTimeout = roomReceiveTimeOut;
                //���̏����͎�M�����܂ŏ�������~����
                client.Receive(ref remote);
                catchText.text = remote.ToString();
            }
            catch
            {
                if (catchText.text == null)
                {
                    catchText.text = "NonCatchException";
                }
            }

            await Task.Delay(roomCatchDelay);
        }

        //var buffer = client.Receive(ref remote);
        //var data = Encoding.UTF8.GetString(buffer);
    }

    public void StopRoom()
    {
        client = null;
        roomBuildContinueFlag = false;
        roomSearchContinueFlag = false;
    }
}
