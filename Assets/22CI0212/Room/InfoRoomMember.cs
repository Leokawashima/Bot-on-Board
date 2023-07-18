using System.Net;
using UnityEngine;
using TMPro;

/// <summary>
/// Memberの情報を格納するクラス
/// </summary>
public class InfoRoomMember : MonoBehaviour
{
    RoomListManager list;

    public IPEndPoint memberEndP { get; private set; }
    public IPAddress memberAddress { get; private set; }
    public string memberName { get; private set; }
    public bool memberReady { get; private set; }

    [SerializeField] TextMeshProUGUI memberNameText;
    [SerializeField] GameObject memberReadyImage;

    public void OnClickInfo()
    {

    }

    public void InitializeInfo(RoomListManager list_, UDPMessage_ConnectRequestData data_)
    {
        list = list_;
        memberEndP = new IPEndPoint(data_.address, RoomUDP.Port);
        memberAddress = data_.address;
        memberName = data_.name;
        memberNameText.text = data_.name;
        memberReady = false;
        memberReadyImage.SetActive(false);
    }

    public void UpdateInfo(RoomListManager list_, MemberData data_)
    {
        list = list_;
        memberEndP = new IPEndPoint(data_.address, RoomUDP.Port);
        memberAddress = data_.address;
        memberName = data_.name;
        memberNameText.text = data_.name;
        memberReady = data_.ready;
        memberReadyImage.SetActive(data_.ready);
    }
}
