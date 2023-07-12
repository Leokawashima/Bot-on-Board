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

    [SerializeField] TextMeshProUGUI memberNameText;

    public void OnClickInfo()
    {

    }

    public void InitializeInfo(ListUIManager list_, byte index_, IPAddress address_, string name_)
    {
        list = list_;
        memberIndex = index_;
        memberAddress = address_;
        memberName = name_;
        memberNameText.text = name_;
    }
}
