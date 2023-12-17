using System;
using UnityEngine;
using TMPro;

public class DeckInfoArea : MonoBehaviour
{
    [SerializeField] private TMP_Text m_categoryText;
    [SerializeField] private TMP_Text m_rankText;
    [SerializeField] private TMP_Text m_deckSizeText;
    [SerializeField] private TMP_InputField m_nameInputField;

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

    public void SetCategoryText(InfoDeckData info_)
    {
        var _text = string.Empty;
        var _colors = new Color[] { Color.red, Color.blue, Color.green, new Color(0.6f, 0.0f, 1.0f), new Color(0.3f, 0.3f, 0.0f) };
        for (int i = 0; i < info_.Data.CategoryCount.Length; ++i)
        {
            _text += $"<color=#{ColorUtility.ToHtmlStringRGB(_colors[i])}>{info_.Data.CategoryCount[i]}</color>\n";
        }
        m_categoryText.text = _text;
    }

    public void SetRankText(InfoDeckData info_)
    {
        var _text = string.Empty;
        var _rank = System.Enum.GetNames(typeof (DeckCardCategory.Rank));
        for (int i = 0; i < info_.Data.RankCount.Length; ++i)
        {
            _text += $"{info_.Data.RankCount[i]}\n";
        }
        m_rankText.text = _text;
    }

    public void SetDeckSizeText(InfoDeckData info_)
    {
        m_deckSizeText.text = info_.Data.Size.ToString();
    }

    public void SetNameText(InfoDeckData info_)
    {
        m_nameInputField.text = info_.Data.Name;
    }
}
