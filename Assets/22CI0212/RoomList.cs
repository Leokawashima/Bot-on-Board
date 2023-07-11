using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;

/// <summary>
/// Roomの接続リストを管理するクラス
/// </summary>
public class RoomList : MonoBehaviour
{
    public RoomInfo selectRoom { get; private set; }
    RectTransform rect;

    [Header("UI")]
    [SerializeField] GameObject connectUI;
    [Header("TextFIeld")]
    [SerializeField] TextMeshProUGUI connectNameText;
    [SerializeField] TextMeshProUGUI connectOptionText;
    [SerializeField] GameObject connectPasswardArea;
    [Header("List")]
    [SerializeField] Transform scrollContent;
    [SerializeField] GameObject infoPrefab;
    [SerializeField] Vector2 startPos = new(-320, 350);
    [SerializeField] Vector2 offsetPos = new(0, -110);
    [SerializeField] List<IPAddress> addressList = new();
    public List<RoomInfo> rooms = new();

    Vector3 StartPos { get { return startPos * transform.lossyScale; } }
    Vector3 OffsetPos { get { return offsetPos * transform.lossyScale; } }

    void Start()
    {
        rect = transform as RectTransform;

        addressList.Clear();
        rooms.Clear();
    }

    public void SetSelectRoomInfo(RoomInfo room_)
    {
        selectRoom = room_;
        connectUI.SetActive(true);
        connectNameText.text = selectRoom.roomName;
        connectOptionText.text = selectRoom.roomOption;
        connectPasswardArea.SetActive(selectRoom.roomPassward);
    }
    public void AddListRoomInfo(IPAddress address_, string[] data)
    {
        if(addressList.Contains(address_)) return;
        var pos = rect.position + StartPos + OffsetPos * rooms.Count;
        var ui = Instantiate(infoPrefab, pos, Quaternion.identity, scrollContent);
        ui.name = "Room_" + data[1];
        var room = ui.GetComponent<RoomInfo>();
        room.InitializeRoomInfo(this, (byte)rooms.Count, data[0], data[1], bool.Parse(data[2]), data[3]);

        addressList.Add(address_);
        rooms.Add(room);
    }
    public void RemMoveListRoomInfo(RoomInfo room_)
    {
        Destroy(room_.gameObject);

        addressList.Remove(room_.roomAddress);
        rooms.Remove(room_);
    }
}
