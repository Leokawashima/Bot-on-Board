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
    string roomPassward;
    [SerializeField] TextMeshProUGUI roomOption;


    public void OnClickInfo()
    {
        roomList.SetSelectRoomInfo(this);
    }

    public void InitializeRoomInfo(RoomList list_, byte index_, string name_, string pass_, string option_)
    {
        roomList = list_;
        roomIndex = index_;
        roomName.text = name_;
        roomPassward = pass_;
        roomOption.text = option_;
    }
}
