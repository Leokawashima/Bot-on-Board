using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class TCPAndUDPTest : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] TMPro.TMP_InputField inputField;

    List<string> Log = new();

    UdpClient udpClient;

    TcpClient tcpClient;
    NetworkStream stream;
    public static IPEndPoint buildEndP { get { return new IPEndPoint(IPAddress.Broadcast, 1111); } }
    public static IPEndPoint searchEndP { get { return new IPEndPoint(IPAddress.Any, 1111); } }
    [ContextMenu("SendTCP")]
    public void SendTCP()
    {
        tcpClient = new TcpClient();
        var local = new IPEndPoint(IPAddress.Parse(inputField.text), 1111);
        tcpClient.Connect(local);

        string message = "TestTCP";

        var buffer = Encoding.UTF8.GetBytes(message);

        stream = tcpClient.GetStream();
        try
        {
            stream.Write(buffer, 0, buffer.Length);
        }
        catch (Exception)
        {
            text.text = "SendTCP Failed";
        }

        text.text = "SendTCP:" + message;
    }

    [ContextMenu("ReceiveTCP")]
    public void ReceiveTCP()
    {
        tcpClient = new TcpClient();
        var local = new IPEndPoint(new IPAddress(new byte[] { 192, 168, 0, 0 }), 1111);
        tcpClient.Connect(local);

        var response = new byte[1024];

        stream = tcpClient.GetStream();
        try
        {
            stream.Read(response, 0, response.Length);
        }
        catch (Exception)
        {
            AddText("ReceiveTCP Failed");
        }

        string message = Encoding.UTF8.GetString(response);

        AddText("ReceiveTCP:" + message);
    }

    [ContextMenu("SendUDP")]
    public async void SendUDP()
    {
        createUDP();

        for (int i = 0; i < 10; ++i)
        {
            string message = "TestUDP";

            var buffer = Encoding.UTF8.GetBytes(message);
            var endP = buildEndP;

            udpClient.Send(buffer, buffer.Length, endP);

            AddText($"SendUCP{i}: " + message);

            await Task.Delay(1000);
        }
    }

    [ContextMenu("ReceiveUDP")]
    public async void ReceiveUDP()
    {
        createUDP();

        for (int i = 0; i < 10; ++i)
        {
            try
            {
                var endP = searchEndP;
                var buffer = udpClient.Receive(ref endP);
                string message = Encoding.UTF8.GetString(buffer);
                AddText($"Receive{i}: " + endP.Address.ToString() + " " + message);
            }
            catch (Exception)
            {
                AddText($"ReceiveUDP Failed{i}");
            }

            await Task.Delay(1000);
        }
    }

    public void Load()
    {
        SceneManager.LoadScene(Name.Scene.Message);
    }

    void createUDP()
    {
        if(udpClient != null) return;

        udpClient = new UdpClient()
        {
            EnableBroadcast = true,
        };
        udpClient.Client.SendTimeout = 1000;
        udpClient.Client.ReceiveTimeout = 1000;
    }

    void AddText(string add)
    {
        if (Log.Count == 10)
        {
            Log.RemoveAt(0);
        }

        Log.Add(add);

        string LogText = "";

        for (int i = 0;  i < Log.Count; ++i)
        {
            LogText += Log[i] + "\n";
        }

        text.text = LogText;
    }
}