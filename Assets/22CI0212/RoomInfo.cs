using System.Net;
using UnityEngine;
using TMPro;

/// <summary>
/// Roomの情報を格納するクラス
/// </summary>
public class RoomInfo : MonoBehaviour
{
    ListUIManager list;

    public byte roomIndex { get; private set; }
    public IPAddress roomAddress { get; private set; }
    public string roomName { get; private set; }
    public string roomOption { get; private set; }
    public bool roomPassward { get; private set; }

    [SerializeField] TextMeshProUGUI roomNameText;
    [SerializeField] TextMeshProUGUI roomOptionText;
    [SerializeField] GameObject roomPassImage;

    public void OnClickInfo()
    {
        list.SetSelectRoomInfo(this);
    }

    public void InitializeInfo(ListUIManager list_, byte index_, UDPMessage_RoomData data_)
    {
        list = list_;
        roomIndex = index_;
        roomAddress = data_.address;
        roomName = data_.name;
        roomNameText.text = data_.name;
        roomPassward = data_.passwardFlag;
        roomPassImage.SetActive(data_.passwardFlag);
        roomOption = data_.option;
        roomOptionText.text = data_.option;
    }
}
