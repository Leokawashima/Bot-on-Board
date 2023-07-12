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

    public void InitializeInfo(ListUIManager list_, byte index_, IPAddress address_, string name_, bool passward_, string option_)
    {
        list = list_;
        roomIndex = index_;
        roomAddress = address_;
        roomName = name_;
        roomNameText.text = name_;
        roomOption = option_;
        roomOptionText.text = option_;
        roomPassward = passward_;
        roomPassImage.SetActive(passward_);
    }
}
