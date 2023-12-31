﻿using System;
using UnityEngine;
using TMPro;

public class DeckInfoArea : MonoBehaviour
{
    [SerializeField] private TMP_Text m_categoryText;
    [SerializeField] private TMP_Text m_rankText;
    [SerializeField] private TMP_Text m_deckSizeText;
    [SerializeField] private TMP_Text m_nameText;

    private void OnEnable()
    {
        DeckListManager.Event_EditOpen += Initialize;
    }
    private void OnDisable()
    {
        DeckListManager.Event_EditOpen -= Initialize;
    }

    private void Initialize(InfoDeckData info_)
    {
        SetCategoryText(info_);
        SetRankText(info_);
        SetDeckSizeText(info_);
        SetNameText(info_);
    }

    private void SetCategoryText(InfoDeckData info_)
    {
        var _text = string.Empty;
        var _category = Enum.GetNames(typeof(DeckCardCategory.Category));
        var _colors = new Color[] { Color.red, Color.blue, Color.green, new Color(0.6f, 0.0f, 1.0f), new Color(0.3f, 0.3f, 0.0f) };
        for (int i = 0; i < info_.Data.CategoryCount.Length; ++i)
        {
            _text += $"<color=#{ColorUtility.ToHtmlStringRGB(_colors[i])}>{_category[i]} {info_.Data.CategoryCount[i]}</color>\n";
        }
        m_categoryText.text = _text;
    }

    private void SetRankText(InfoDeckData info_)
    {
        var _text = string.Empty;
        var _rank = Enum.GetNames(typeof (DeckCardCategory.Rank));
        for (int i = 0; i < info_.Data.RankCount.Length; ++i)
        {
            _text += $"{_rank[i]} {info_.Data.RankCount[i]}\n";
        }
        m_rankText.text = _text;
    }

    private void SetDeckSizeText(InfoDeckData info_)
    {
        m_deckSizeText.text = info_.Data.Size.ToString();
    }

    private void SetNameText(InfoDeckData info_)
    {
        m_nameText.text = info_.Data.Name;
    }
}
