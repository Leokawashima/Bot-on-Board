using UnityEngine;
using TMPro;
using Map;

public class DeckListInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;

    public void SetInfo(InfoDeckData info_)
    {
        var _text = $"Name = {info_.Data.Name}\n";

        if (info_.Data.Cards != null)
        {
            _text += $"State = {info_.Data.State}\n";
            _text += $"Size = {info_.Data.Cards.Count}\n";

            for (int i = 0, cnt = info_.Data.Cards.Count; i < cnt; ++i)
            {
                if (info_.Data.Cards[i] != -1)
                {
                    _text += MapTable.Object.Table[info_.Data.Cards[i]].Name + "\n";
                }
                else
                {
                    _text += "ナシ\n";
                }
            }
        }

        m_text.text = _text;
    }
}