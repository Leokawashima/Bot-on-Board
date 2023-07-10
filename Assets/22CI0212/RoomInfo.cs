using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomInfo : MonoBehaviour
{
    RoomList roomList;

    public byte roomIndex { get; private set; }
    public string address { get; private set; }

    [SerializeField] TextMeshProUGUI roomName;
    [SerializeField] TextMeshProUGUI roomOption;
    [SerializeField] GameObject roomPassImage;

    public void OnClickInfo()
    {
        roomList.SetSelectRoomInfo(this);
    }

    public void InitializeRoomInfo(RoomList list_, byte index_, string address_, string name_, bool passward_, string option_)
    {
        roomList = list_;
        roomIndex = index_;
        address = address_;
        roomName.text = name_;
        roomOption.text = option_;
        roomPassImage.SetActive(passward_);
    }
}
