using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RoomUDPSystem;

/// <summary>
/// Member単位の情報を表示、保持するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class InfoRoomMember : MonoBehaviour
{
    MemberData m_memberData;
    public MemberData MemberData => m_memberData;

    // 送受信ではIPAddressからではなくEndPointを経由しなければいけないので
    // キャッシュ変数を持たせている
    public IPEndPoint EndPoint { get; private set; }
     
    [SerializeField] Button m_button;
    [SerializeField] TMP_Text m_nameText;
    [SerializeField] Image m_readyImage;

    public event Action<InfoRoomMember> Event_Button;

    void Initialize(IPAddress address_, string name_, bool isReady_ = false)
    {
        // フィールド設定
        m_memberData = new MemberData(address_, name_, false);
        EndPoint = new IPEndPoint(address_, RoomUDP.Port);
        // イベントハンドラをボタンに登録
        m_button.onClick.AddListener(OnButton);

        // 表示情報設定
        m_nameText.text = name_;
        m_readyImage.enabled = isReady_;
    }
    public void Initialize(MemberData membderData_)
    {
        Initialize(membderData_.Address, membderData_.Name, membderData_.IsReady);
    }
    public void Initialize(ConnectionRequestData requestData_)
    {
        Initialize(requestData_.Address, requestData_.Name);
    }

    /// <summary>
    /// 表示情報を更新する関数
    /// </summary>
    /// <param name="memberData_">更新に使用するMmeberの情報</param>
    public void UpdateReady(MemberData memberData_)
    {
        // 準備完了情報を更新
        m_memberData.SetIsReady(memberData_.IsReady);
        m_readyImage.enabled = memberData_.IsReady;
    }

    /// <summary>
    /// ボタンが押された時のイベントハンドラ
    /// </summary>
    void OnButton()
    {
        Event_Button?.Invoke(this);
    }
}