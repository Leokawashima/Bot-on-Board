using UnityEngine;
using TMPro;

public class ShowInfoPlayerSetting : MonoBehaviour
{
    [SerializeField] private TMP_Text m_indexText;
    [SerializeField] private TMP_InputField m_nameInputField;
    [field: SerializeField] public PlusMinusButton PlusMinusButton { get; private set; }

    public void Select(InfoPlayerSetting info_)
    {
        m_indexText.text = info_.Data.Index.ToString();
        m_nameInputField.text = info_.Data.Name.ToString();
    }
}