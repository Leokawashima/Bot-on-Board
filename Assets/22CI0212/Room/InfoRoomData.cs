using System.Net;
using UnityEngine;
using TMPro;

/// <summary>
/// Roomの情報を格納するクラス
/// </summary>
public class InfoRoomData : MonoBehaviour
{
    RoomListManager list;

    public RoomData roomData = new();

    [SerializeField] TextMeshProUGUI roomNameText;
    [SerializeField] TextMeshProUGUI roomOptionText;
    [SerializeField] GameObject roomPasswardImage;
    [SerializeField] TextMeshProUGUI roomUserText;

    public void OnClickInfo()
    {
        list.SetSelectRoomInfo(this);
    }

    public void SetInfo(RoomListManager list_, UDPMessage_RoomData data_)
    {
        list = list_;
        roomData.address = data_.address;
        roomData.name = data_.name;
        roomNameText.text = data_.name;
        roomData.passward = data_.passwardFlag;
        roomPasswardImage.SetActive(data_.passwardFlag);
        roomData.option = data_.option;
        roomOptionText.text = data_.option;
        roomData.userMax = data_.userMax;//最大人数
        roomData.userCnt = data_.userCnt;//現在の人数
        roomUserText.text = data_.userMax + "/" + data_.userCnt; 
    }
}

public struct RoomData
{
    public IPAddress address;
    public string name;
    public string option;
    public bool passward;
    public int userMax;
    public int userCnt;
}