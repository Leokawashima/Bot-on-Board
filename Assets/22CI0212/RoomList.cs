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
    [SerializeField] GameObject memberPrefab;
    [SerializeField] uint people = 2;
    [SerializeField] Vector2 startPos = new Vector2(-320, 240);
    [SerializeField] Vector2 offsetPos = new Vector2(0, -110);

    void Start()
    {
        rect = transform as RectTransform;
        Vector3 start = startPos * transform.lossyScale;
        Vector3 offset = offsetPos * transform.lossyScale;

        for (int i = 0; i < people; ++i)
        {
            var pos = rect.position + start + offset * i;
            var ui = Instantiate(memberPrefab, pos, Quaternion.identity, scrollContent);
            var info = ui.GetComponent<RoomInfo>();
            info.roomName.text = "Test";
            info.roomOption.text = "TestOptions";
        }
    }
}
