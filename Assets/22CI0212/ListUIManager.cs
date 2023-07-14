using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;

/// <summary>
/// 接続リストUIを管理するクラス
/// </summary>
public class ListUIManager : MonoBehaviour
{
    #region Field
    public RoomInfo selectRoom { get; private set; }
    RectTransform rect;

    [Header("COnnect")]
    [SerializeField] GameObject connectUI;
    [SerializeField] TextMeshProUGUI connectNameText;
    [SerializeField] TextMeshProUGUI connectOptionText;
    [SerializeField] GameObject connectPasswardArea;
    [Header("List")]
    [SerializeField] Transform scrollContent;
    [Header("InfoPrefab")]
    [SerializeField] GameObject roomInfoPrefab;
    [SerializeField] GameObject memberInfoPrefab;
    [Header("Position")]
    [SerializeField] Vector2 startPos = new(-320, 350);
    [SerializeField] Vector2 offsetPos = new(0, -110);
    [Header("Debug")]
    [SerializeField] List<IPAddress> addressList = new();
    public List<RoomInfo> rooms { get; private set; } = new();
    public List<MemberInfo> members { get; private set; } = new();

    Vector3 StartPos { get { return startPos * transform.lossyScale; } }
    Vector3 OffsetPos { get { return offsetPos * transform.lossyScale; } }
    #endregion

    public void InitializeList()
    {
        rect = transform as RectTransform;

        addressList.Clear();
        rooms.Clear();
        members.Clear();
    }

    public void SetSelectRoomInfo(RoomInfo room_)
    {
        selectRoom = room_;
        connectUI.SetActive(true);
        connectNameText.text = selectRoom.roomName;
        connectOptionText.text = selectRoom.roomOption;
        connectPasswardArea.SetActive(selectRoom.roomPassward);
    }

    public void AddListRoomInfo(IPAddress address_, string[] data_)
    {
        if (addressList.Contains(address_))
        {
#if UNITY_EDITOR
            Debug.Log("Address has already connected");
#endif
            return;
        }
        var pos = rect.position + StartPos + OffsetPos * rooms.Count;
        var ui = Instantiate(roomInfoPrefab, pos, Quaternion.identity, scrollContent);
        ui.name = "Room_" + data_[1];
        var room = ui.GetComponent<RoomInfo>();
        room.InitializeInfo(this, (byte)rooms.Count, IPAddress.Parse(data_[0]), data_[1], bool.Parse(data_[2]), data_[3]);

        addressList.Add(address_);
        rooms.Add(room);
    }
    public void RemoveListRoomInfo(RoomInfo room_)
    {
        Destroy(room_.gameObject);

        addressList.Remove(room_.roomAddress);
        rooms.Remove(room_);
    }
    public void AddListMemberInfo(IPAddress address_, string[] data_)
    {
        if (addressList.Contains(address_))
        {
#if UNITY_EDITOR
            Debug.Log("Address has already connected");
#endif
            return;
        }
        var pos = rect.position + StartPos + OffsetPos * members.Count;
        var ui = Instantiate(memberInfoPrefab, pos, Quaternion.identity, scrollContent);
        ui.name = "Member_" + data_[1];
        var member = ui.GetComponent<MemberInfo>();
        member.InitializeInfo(this, (byte)members.Count, IPAddress.Parse(data_[0]), data_[1]);

        addressList.Add(address_);
        members.Add(member);
    }
    public void RemoveListMemberInfo(MemberInfo member_)
    {
        Destroy(member_.gameObject);

        addressList.Remove(member_.memberAddress);
        members.Remove(member_);
    }
}
