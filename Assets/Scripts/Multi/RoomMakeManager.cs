using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Roomを作る(Hostする)のを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class RoomMakeManager : MonoBehaviour
{
    [SerializeField] TMP_InputField m_nameText;
    public string Name => m_nameText.text;

    [SerializeField] TMP_InputField m_optionText;
    public string Option => m_optionText.text;

    [SerializeField] Toggle m_passwardToggle;
    public bool HasPassward => m_passwardToggle.isOn;

    [SerializeField] TMP_InputField m_passwardInputField;
    public int Passward => int.Parse(m_passwardInputField.text);

    [SerializeField] TMP_InputField m_userMaxInputField;
    public int UserMax => int.Parse(m_userMaxInputField.text);
}