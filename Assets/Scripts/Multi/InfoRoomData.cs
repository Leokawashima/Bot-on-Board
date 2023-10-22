using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RoomUDPSystem;

/// <summary>
/// Room単位の情報を表示、保持するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class InfoRoomData : MonoBehaviour
{
    RoomData m_roomData;
    public RoomData RoomData => m_roomData;

    [SerializeField] Button m_button;
    [SerializeField] TMP_Text m_nameText;
    [SerializeField] TMP_Text m_optionText;
    [SerializeField] Image m_passwardImage;
    [SerializeField] TMP_Text m_UserText;

    public event Action<InfoRoomData> Event_Button;

    /// <summary>
    /// 初期化関数
    /// </summary>
    /// <param name="roomData_">初期化に使用するRoomの情報</param>
    public void Initialize(RoomData roomData_)
    {
        // フィールド設定
        m_roomData = roomData_;
        // イベントハンドラをボタンに登録
        m_button.onClick.AddListener(OnButton);

        // 表示情報設定
        m_nameText.text = roomData_.Name;
        m_passwardImage.enabled = (roomData_.HasPassward);
        m_optionText.text = roomData_.Option;
        m_UserText.text = roomData_.UserMax + "/" + roomData_.UserCount;
    }

    /// <summary>
    /// ボタンが押された時のイベントハンドラ
    /// </summary>
    void OnButton()
    {
        Event_Button?.Invoke(this);
    }
}