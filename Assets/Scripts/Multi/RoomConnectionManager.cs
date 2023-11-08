using UnityEngine;
using TMPro;
using RoomUDPSystem;

/// <summary>
/// 接続するルームの情報を表示、管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class RoomConnectionManager : MonoBehaviour
{
    [field: SerializeField] public TMP_Text NameText { get; private set; }
    [field: SerializeField] public TMP_Text OptionText { get; private set; }
    [field: SerializeField] public GameObject PasswardArea { get; private set; }
    [field: SerializeField] public TMP_InputField PasswardInputField { get; private set; }

    public void Enable()
    {
        gameObject.SetActive(false);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void SetInfo(RoomData roomData_)
    {
        NameText.text = roomData_.Name;
        OptionText.text = roomData_.Option;
        PasswardArea.SetActive(roomData_.HasPassward);
    }
}