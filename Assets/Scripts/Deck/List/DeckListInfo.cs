using UnityEngine;
using TMPro;
using Map;

namespace Deck
{
    public class DeckListInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_nameText;
        [SerializeField] private TMP_Text m_infoText;
        [SerializeField] private TMP_Text m_categoryText;
        [SerializeField] private TMP_Text m_RarityText;
        [SerializeField] private TMP_Text m_cardsText;

        public void SetInfo(InfoDeckData info_)
        {
            m_nameText.text = info_.Data.Name;
            if (info_.Data.Category != null)
            {
                m_categoryText.text = info_.Data.Category.Length.ToString();
            }
            if (info_.Data.Rarity != null)
            {
                m_RarityText.text = info_.Data.Rarity.Length.ToString();
            }
            if (info_.Data.Cards != null)
            {
                m_infoText.text = string.Empty;
                m_infoText.text += $"State = {info_.Data.State}\n";
                m_infoText.text += $"Size = {info_.Data.Cards.Count}\n";

                var _text = string.Empty;
                for (int i = 0, cnt = info_.Data.Cards.Count; i < cnt; ++i)
                {
                    _text += info_.Data.Cards[i] != -1 ?
                        $"{MapTable.Object.Table[info_.Data.Cards[i]].Name}\n" :
                        "ナシ\n";
                }
                m_cardsText.text = _text;
            }
        }
    }
}