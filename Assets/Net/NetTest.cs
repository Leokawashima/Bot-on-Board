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
    [SerializeField] bool roomBuildEndFlag = true;
    [SerializeField] IPAddress buildAddress = IPAddress.Broadcast;//new IPAddress(new byte[4] { 127, 0, 0, 1})
    [SerializeField] int roomSendDelay = 5000;
    [SerializeField] int roomSendTimeOut = 1000;
    [Header("Search")]
    [SerializeField] bool roomSearchContinueFlag = false;
    [SerializeField] bool roomSearchEndFlag = true;
    [SerializeField] IPAddress SearchAdderess = IPAddress.Any;//new IPAddress(new byte[4] { 127, 0, 0, 1 })
    [SerializeField] int roomCatchDelay = 5000;
    [SerializeField] int roomReceiveTimeOut = 500;
    [Header("TextField")]
    [SerializeField] TextMeshProUGUI catchText;
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] uint logSize = 20;
    List<string> logStr = new List<string>();

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
                return ip.ToString();
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
                LogPush("Error : SocketException");
            }

            LogPush("search completed... waiting for delay");
            await Task.Delay(roomCatchDelay);
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
}
