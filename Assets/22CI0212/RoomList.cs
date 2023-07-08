using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Roomの接続リストを管理するクラス
/// </summary>
public class RoomList : MonoBehaviour
{
    RectTransform rect;

    [Header("List")]
    [SerializeField] Transform scrollContent;
    [SerializeField] GameObject infoPrefab;
    [SerializeField] Vector2 startPos = new Vector2(-320, 240);
    [SerializeField] Vector2 offsetPos = new Vector2(0, -110);

    Vector3 StartPos { get { return startPos * transform.lossyScale; } }
    Vector3 OffsetPos { get { return offsetPos * transform.lossyScale; } }

    void Start()
    {
        rect = transform as RectTransform;
    }

    public void SetRoomInfo(string[] data_)
    {
        for(int i = 0; i < 2; ++i)
        {
            var pos = rect.position + StartPos + OffsetPos * i;
            var ui = Instantiate(infoPrefab, pos, Quaternion.identity, scrollContent);
            var info = ui.GetComponent<RoomInfo>();
            info.roomName.text = "Test";
            info.roomOption.text = "TestOptions";
        }
    }
}
