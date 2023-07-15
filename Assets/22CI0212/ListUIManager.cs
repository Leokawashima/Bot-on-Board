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

    public List<IPAddress> addressList { get; private set; } = new();
    public List<RoomInfo> rooms { get; private set; } = new();
    public List<MemberInfo> members { get; private set; } = new();

    Vector3 StartPos { get { return startPos * transform.lossyScale; } }
    Vector3 OffsetPos { get { return offsetPos * transform.lossyScale; } }
    #endregion

    public void Initialize()
    {
        rect = transform as RectTransform;

        Clear();
    }
    public void Clear()
    {
        foreach(var room in rooms)
            Destroy(room.gameObject);
        foreach(var member in members)
            Destroy(member.gameObject);

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

    public void Add_RoomInfo(IPAddress address_, UDPMessage_RoomData data_)
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
        ui.name = "Room_" + data_.name;
        var room = ui.GetComponent<RoomInfo>();
        room.SetInfo(this, (byte)rooms.Count, data_);

        addressList.Add(address_);
        rooms.Add(room);
    }
    public void Remove_RoomInfo(RoomInfo room_)
    {
        Destroy(room_.gameObject);

        addressList.Remove(room_.roomAddress);
        rooms.Remove(room_);
    }
    public void RemoveAll_RoomInfo()
    {
        foreach(var room in rooms)
            Destroy(room.gameObject);

        addressList.Clear();
        rooms.Clear();
    }
    public void Add_MemberInfo(IPAddress address_, string name_)
    {
        if(addressList.Contains(address_))
        {
#if UNITY_EDITOR
            Debug.Log("Address has already connected");
#endif
            return;
        }
        var pos = rect.position + StartPos + OffsetPos * members.Count;
        var ui = Instantiate(memberInfoPrefab, pos, Quaternion.identity, scrollContent);
        ui.name = "Member_" + name_;
        var member = ui.GetComponent<MemberInfo>();
        member.SetInfo(this, (byte)members.Count, address_, name_);

        addressList.Add(address_);
        members.Add(member);
    }
    public void Add_MemberInfo(MemberInfoData data_)
    {
        if(addressList.Contains(data_.address))
        {
#if UNITY_EDITOR
            Debug.Log("Address has already connected");
#endif
            return;
        }
        var pos = rect.position + StartPos + OffsetPos * members.Count;
        var ui = Instantiate(memberInfoPrefab, pos, Quaternion.identity, scrollContent);
        ui.name = "Member_" + data_.name;
        var member = ui.GetComponent<MemberInfo>();
        member.SetInfo(this, (byte)members.Count, data_);

        addressList.Add(data_.address);
        members.Add(member);
    }
    public void Add_MemberInfo(IPAddress address_, UDPMessage_ConnectRequestData data_)
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
        ui.name = "Member_" + data_.name;
        var member = ui.GetComponent<MemberInfo>();
        member.SetInfo(this, (byte)members.Count, data_);

        addressList.Add(address_);
        members.Add(member);
    }
    public void Remove_MemberInfo(MemberInfo member_)
    {
        Destroy(member_.gameObject);

        addressList.Remove(member_.memberAddress);
        members.Remove(member_);
    }
    public void RemoveAll_MemberInfo()
    {
        foreach(var member in members)
            Destroy(member.gameObject);

        addressList.Clear();
        members.Clear();
    }
}
