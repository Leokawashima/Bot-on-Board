using System.Net;
using UnityEngine;
using TMPro;

/// <summary>
/// Memberの情報を格納するクラス
/// </summary>
public class MemberInfo : MonoBehaviour
{
    ListUIManager list;
    
    public byte memberIndex { get; private set; }
    public IPAddress memberAddress { get; private set; }
    public string memberName { get; private set; }
    public bool memberReady { get; private set; }

    [SerializeField] TextMeshProUGUI memberNameText;
    [SerializeField] GameObject memberReadyImage;

    public void OnClickInfo()
    {

    }

    public void SetInfo(ListUIManager list_, byte index_, IPAddress address_, string name_)
    {
        list = list_;
        memberIndex = index_;
        memberAddress = address_;
        memberName = name_;
        memberNameText.text = name_;
        memberReady = false;
        memberReadyImage.SetActive(false);
    }

    public void SetInfo(ListUIManager list_, byte index_, UDPMessage_ConnectRequestData data_)
    {
        list = list_;
        memberIndex = index_;
        memberAddress = data_.address;
        memberName = data_.name;
        memberNameText.text = data_.name;
        memberReady = false;
        memberReadyImage.SetActive(false);
    }

    public void SetInfo(ListUIManager list_, byte index_, MemberInfoData data_)
    {
        list = list_;
        memberIndex = index_;
        memberAddress = data_.address;
        memberName = data_.name;
        memberNameText.text = data_.name;
        memberReady = data_.ready;
        memberReadyImage.SetActive(data_.ready);
    }
}
