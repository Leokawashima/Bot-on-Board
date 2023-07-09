using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Roomの接続リストを管理するクラス
/// </summary>
public class RoomList : MonoBehaviour
{
    RoomInfo selectInfo;
    RectTransform rect;

    [Header("List")]
    [SerializeField] Transform scrollContent;
    [SerializeField] GameObject infoPrefab;
    [SerializeField] Vector2 startPos = new Vector2(-320, 350);
    [SerializeField] Vector2 offsetPos = new Vector2(0, -110);
    [SerializeField] List<string> addressList = new();
    [SerializeField] List<RoomInfo> rooms = new();

    Vector3 StartPos { get { return startPos * transform.lossyScale; } }
    Vector3 OffsetPos { get { return offsetPos * transform.lossyScale; } }

    void Start()
    {
        rect = transform as RectTransform;

        addressList.Clear();
        rooms.Clear();
    }

    public void SetSelectRoomInfo(RoomInfo info_)
    {
        selectInfo = info_;
    }
    public void SetListRoomInfo(string[] data)
    {
        if(addressList.Contains(data[0])) return;
        var pos = rect.position + StartPos + OffsetPos * rooms.Count;
        var ui = Instantiate(infoPrefab, pos, Quaternion.identity, scrollContent);
        var info = ui.GetComponent<RoomInfo>();
        info.InitializeRoomInfo(this, (byte)rooms.Count, data[0], data[1], data[2]);

        addressList.Add(data[0]);
        rooms.Add(info);
    }
}
