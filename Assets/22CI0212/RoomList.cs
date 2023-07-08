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
    public static RoomList singleton { get; private set; }
    RoomInfo selectInfo;
    RectTransform rect;

    [Header("List")]
    [SerializeField] Transform scrollContent;
    [SerializeField] GameObject infoPrefab;
    [SerializeField] Vector2 startPos = new Vector2(-320, 240);
    [SerializeField] Vector2 offsetPos = new Vector2(0, -110);
    [SerializeField] List<RoomInfo> rooms = new();

    Vector3 StartPos { get { return startPos * transform.lossyScale; } }
    Vector3 OffsetPos { get { return offsetPos * transform.lossyScale; } }

    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
        rect = transform as RectTransform;
    }

    public void SetSelectRoomInfo(RoomInfo info_, Image button_)
    {
        selectInfo = info_;
    }
    public void SetRoomInfo(string[] data)
    {
        var pos = rect.position + StartPos + OffsetPos;
        var ui = Instantiate(infoPrefab, pos, Quaternion.identity, scrollContent);
        var info = ui.GetComponent<RoomInfo>();
        info.roomIndex = (byte)rooms.Count;
        info.roomName.text = data[0];
        info.roomPassward = ushort.Parse(data[1]);
        info.roomOption.text = data[2];
        rooms.Add(info);
    }
}
