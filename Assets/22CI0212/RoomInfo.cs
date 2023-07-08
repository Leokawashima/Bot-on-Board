using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomInfo : MonoBehaviour
{
    public byte roomIndex;
    public TextMeshProUGUI roomName;
    public ushort roomPassward;
    public TextMeshProUGUI roomOption;

    [SerializeField] Image button;

    public void OnClickInfo()
    {
        RoomList.singleton.SetSelectRoomInfo(this, button);
    }
}
