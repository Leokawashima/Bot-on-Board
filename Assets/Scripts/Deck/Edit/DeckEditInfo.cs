using UnityEngine;
using TMPro;
using Map;
using Deck.List;

namespace Deck.Edit
{
    /// <summary>
    /// 編集画面の明細情報を表示するクラス
    /// </summary>
    public class DeckEditInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_categoryText;
        [SerializeField] private TMP_Text m_rankText;
        [SerializeField] private TMP_Text m_deckSizeText;
        [SerializeField] private TMP_InputField m_nameInputField;
        public string NameText => m_nameInputField.text;

        private DeckData m_deck;

        public void Initialize()
        {
            m_deck = new()
            {
                Rarity = new int[4],
                Category = new int[5],
                Cards = new(),
            };
            DeckEditManager.Singleton.DragManager.Event_EndDrag += OnEndDrag;
            DeckListManager.Singleton.Event_Edit += OnEdit;
        }

        private void OnEndDrag(MapObjectCard card_, int index_)
        {
            if (DeckEditManager.Singleton.DragManager.EditDeck.Cards[index_] != -1)
            {
                var _mo = MapTable.Object.Table[DeckEditManager.Singleton.DragManager.EditDeck.Cards[index_]];
                //m_deck.Rank[(int)_mo.HasRarity]--;
                //m_deck.Category[(int)_mo.Category]--;
                m_deck.Cards.Remove(card_.Index);
            }

            //m_deck.Rank[(int)card_.SO.HasRarity]++;
            //m_deck.Category[(int)card_.SO.Category]++;
            m_deck.Cards.Add(card_.Index);
            SetCategoryText(m_deck);
            SetRankText(m_deck);
            SetDeckSizeText(m_deck);
        }

        private void OnEdit(InfoDeckData info_)
        {
            SetCategoryText(info_.Data);
            SetRankText(info_.Data);
            SetDeckSizeText(info_.Data);
            SetNameText(info_.Data);
        }

        public void SetCategoryText(DeckData deck_)
        {
            var _text = string.Empty;
            deck_.Category = new int[1];
            
            for (int i = 0; i < deck_.Category.Length; ++i)
            {
                _text += $"{deck_.Category[i]}\n";
            }
            m_categoryText.text = _text;
        }

        public void SetRankText(DeckData deck_)
        {
            var _text = string.Empty;
            deck_.Rarity = new int[1];

            for (int i = 0; i < deck_.Rarity.Length; ++i)
            {
                _text += $"{deck_.Rarity[i]}\n";
            }
            m_rankText.text = _text;
        }

        public void SetDeckSizeText(DeckData deck_)
        {
            m_deckSizeText.text = deck_.Cards.Count.ToString();
        }

        public void SetNameText(DeckData deck_)
        {
            m_nameInputField.text = deck_.Name;
        }
    }
}