using UnityEngine;
using TMPro;

public class DeckListInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;

    public void SetInfo(InfoDeckData info_)
    {
        m_text.text = $@"
Name = {info_.Data.Name}
State = {info_.Data.State}
Size = {info_.Data.Size}
";
    }
}