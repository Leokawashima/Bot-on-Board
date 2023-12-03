using UnityEngine;
using TMPro;

public class DeckListInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;

    [SerializeField] private MapObjectTable_SO m_table;

    public void SetInfo(InfoDeckData info_)
    {
        var _text = $"Name = {info_.Data.Name}\n";
        if (info_.Data.CardIndexArray != null)
        {
            _text += $"State = {info_.Data.State}\n";
            _text += $"Size = {info_.Data.Size}\n";

            if(info_.Data.CardIndexArray != null)
            {
                for(int i = 0; i < info_.Data.CardIndexArray.Length; ++i)
                {
                    _text += m_table.Data[info_.Data.CardIndexArray[i]].m_ObjectName + "\n";
                }
            }
        }

        m_text.text = _text;
    }
}