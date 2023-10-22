using System.Net;
using System.Collections.Generic;
using UnityEngine;
using RoomUDPSystem;

/// <summary>
/// Roomのリストを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class RoomListManager : MonoBehaviour
{
    #region Field
    [Header("Ccnnect")]
    [SerializeField] RoomConnectionManager m_roomConnecttionManager;
    [Header("List")]
    [SerializeField] RectTransform scrollContent;
    [Header("InfoPrefab")]
    [SerializeField] InfoRoomData roomInfoPrefab;
    [SerializeField] InfoRoomMember memberInfoPrefab;

#if UNITY_EDITOR
    [field: Header("Debug"),SerializeField]
#endif
    public InfoRoomData SelectInfoRoomData { get; private set; }

    public List<IPAddress> AddressList { get; private set; } = new();
    public List<InfoRoomData> RoomList { get; private set; } = new();
    public List<InfoRoomMember> MemberList { get; private set; } = new();

    #endregion

    public void Clear()
    {
        foreach(var room in RoomList)
            Destroy(room.gameObject);
        foreach(var member in MemberList)
            Destroy(member.gameObject);

        AddressList.Clear();
        RoomList.Clear();
        MemberList.Clear();
    }

    bool CheckContainsAddressList(IPAddress address_)
    {
#if UNITY_EDITOR
        if(AddressList.Contains(address_))
        {
            Debug.Log("Address has already connected");
            return true;
        }
        return false;
#else
        return AddressList.Contains(address_);
#endif
    }

    public void AddRoomInfo(IPAddress address_, RoomData data_)
    {
        if(CheckContainsAddressList(address_))
            return;

        var _room = Instantiate(roomInfoPrefab, scrollContent);
        _room.name = "Room_" + data_.Name;
        _room.Initialize(data_);
        _room.Event_Button += _OnSelectRoomInfo;

        AddressList.Add(address_);
        RoomList.Add(_room);

        void _OnSelectRoomInfo(InfoRoomData roomData_)
        {
            SelectInfoRoomData = roomData_;
            m_roomConnecttionManager.Enable();
            m_roomConnecttionManager.SetInfo(roomData_.RoomData);
        }
    }
    public void RemoveRoomInfo(InfoRoomData room_)
    {
        AddressList.Remove(room_.RoomData.Address);
        RoomList.Remove(room_);

        Destroy(room_.gameObject);
    }
    public void ClearRoomInfo()
    {
        foreach(var room in RoomList)
            Destroy(room.gameObject);

        AddressList.Clear();
        RoomList.Clear();
    }

    public void AddMemberInfo(MemberData data_)
    {
        if(CheckContainsAddressList(data_.Address))
            return;

        var _member = Instantiate(memberInfoPrefab, scrollContent);
        _member.name = "Member_" + data_.Name;
        _member.Initialize(data_);

        AddressList.Add(data_.Address);
        MemberList.Add(_member);
    }
    public void AddMemberInfo(IPAddress address_, ConnectionRequestData data_)
    {
        if(CheckContainsAddressList(address_))
            return;

        var _member = Instantiate(memberInfoPrefab, scrollContent);
        _member.name = "Member_" + data_.Name;
        _member.Initialize(data_);

        AddressList.Add(address_);
        MemberList.Add(_member);
    }
    public void RemoveMemberInfo(InfoRoomMember member_)
    {
        AddressList.Remove(member_.EndPoint.Address);
        MemberList.Remove(member_);

        Destroy(member_.gameObject);
    }
    public void ClearMemberInfo()
    {
        foreach(var member in MemberList)
            Destroy(member.gameObject);

        AddressList.Clear();
        MemberList.Clear();
    }
}