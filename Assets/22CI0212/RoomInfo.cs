using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;

public class RoomInfo : MonoBehaviour
{
    RoomList roomList;

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
        roomList.SetSelectRoomInfo(this);
    }

    public void InitializeRoomInfo(RoomList list_, byte index_, string address_, string name_, bool passward_, string option_)
    {
        roomList = list_;
        roomIndex = index_;
        roomAddress = IPAddress.Parse(address_);
        roomName = name_;
        roomNameText.text = name_;
        roomOption = option_;
        roomOptionText.text = option_;
        roomPassward = passward_;
        roomPassImage.SetActive(passward_);
    }
}
