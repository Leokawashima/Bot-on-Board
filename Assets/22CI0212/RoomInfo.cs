using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomInfo : MonoBehaviour
{
    RoomList roomList;

    byte roomIndex;
    [SerializeField] TextMeshProUGUI roomName;
    [SerializeField] TextMeshProUGUI roomOption;
    [SerializeField] GameObject roomPassImage;

    public void OnClickInfo()
    {
        roomList.SetSelectRoomInfo(this);
    }

    public void InitializeRoomInfo(RoomList list_, byte index_, string name_, bool passward_, string option_)
    {
        roomList = list_;
        roomIndex = index_;
        roomName.text = name_;
        roomOption.text = option_;
        roomPassImage.SetActive(passward_);
    }
}
