using System;
using UnityEngine;
using TMPro;

public class DeckInfoArea : MonoBehaviour
{
    [SerializeField] private TMP_Text m_categoryText;
    [SerializeField] private TMP_Text m_rankText;
    [SerializeField] private TMP_Text m_DeckSizeText;

    [SerializeField] private DeckData m_deck;

    private void Start()
    {
        m_categoryText.text = string.Empty;
        var _text = string.Empty;
        var _category = Enum.GetNames(typeof(DeckCardCategory.Category));
        for(int i = 0; i < m_deck.CategoryCount.Length; ++i)
        {
            _text += $"<color=red>{_category[i]} {m_deck.CategoryCount[i]}</color>";
        }
    }
}
