using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;

/// <summary>
/// 接続リストUIを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class RoomListManager : MonoBehaviour
{
    #region Field
    public InfoRoomData selectRoom { get; private set; }
    RectTransform rect;

    [Header("Ccnnect")]
    [SerializeField] RoomConnectManager roomConnect;
    
    [Header("List")]
    [SerializeField] RectTransform scrollContent;
    [Header("InfoPrefab")]
    [SerializeField] GameObject roomInfoPrefab;
    [SerializeField] GameObject memberInfoPrefab;
    [Header("Position")]
    [SerializeField] Vector2 startPos = new(-320, -55);
    [SerializeField] Vector2 offsetPos = new(0, -110);

    public List<IPAddress> addressList { get; private set; } = new();
    public List<InfoRoomData> rooms { get; private set; } = new();
    public List<InfoRoomMember> members { get; private set; } = new();

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

    public void SetSelectRoomInfo(InfoRoomData room_)
    {
        selectRoom = room_;
        roomConnect.gameObject.SetActive(true);
        roomConnect.getNameText.text = selectRoom.roomData.name;
        roomConnect.getOptionText.text = selectRoom.roomData.option;
        roomConnect.getPasswardArea.SetActive(selectRoom.roomData.passward);
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
        pos.y += scrollContent.position.y;
        var ui = Instantiate(roomInfoPrefab, pos, Quaternion.identity, scrollContent);
        ui.name = "Room_" + data_.name;
        var room = ui.GetComponent<InfoRoomData>();
        room.SetInfo(this, data_);

        addressList.Add(address_);
        rooms.Add(room);
    }
    public void Remove_RoomInfo(InfoRoomData room_)
    {
        Destroy(room_.gameObject);

        addressList.Remove(room_.roomData.address);
        rooms.Remove(room_);
    }
    public void RemoveAll_RoomInfo()
    {
        foreach(var room in rooms)
            Destroy(room.gameObject);

        addressList.Clear();
        rooms.Clear();
    }

    public void Add_MemberInfo(MemberData data_)
    {
        if(addressList.Contains(data_.address))
        {
#if UNITY_EDITOR
            Debug.Log("Address has already connected");
#endif
            return;
        }
        var pos = rect.position + StartPos + OffsetPos * members.Count;
        pos.y += scrollContent.position.y;
        var ui = Instantiate(memberInfoPrefab, pos, Quaternion.identity, scrollContent);
        ui.name = "Member_" + data_.name;
        var member = ui.GetComponent<InfoRoomMember>();
        member.UpdateInfo(this, data_);

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
        pos.y += scrollContent.position.y;
        var ui = Instantiate(memberInfoPrefab, pos, Quaternion.identity, scrollContent);
        ui.name = "Member_" + data_.name;
        var member = ui.GetComponent<InfoRoomMember>();
        member.InitializeInfo(this, data_);

        addressList.Add(address_);
        members.Add(member);
    }
    public void Remove_MemberInfo(InfoRoomMember member_)
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