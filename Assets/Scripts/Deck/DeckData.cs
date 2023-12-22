using System;
using System.Collections.Generic;

[Serializable]
public class DeckData
{
    /// <summary>
    /// デッキの名前
    /// </summary>
    public string Name = "null";

    /// <summary>
    /// デッキの状態
    /// </summary>
    public DeckManager.DeckState State = DeckManager.DeckState.Non;

    /// <summary>
    /// デッキのカテゴリ毎のカウント
    /// </summary>
    public int[] CategoryCount = new int[Enum.GetValues(typeof(DeckCardCategory.Category)).Length];

    /// <summary>
    /// デッキのランク毎のカウント
    /// </summary>
    public int[] RankCount = new int[Enum.GetValues(typeof(DeckCardCategory.Rank)).Length];

    /// <summary>
    /// デッキのフィールド
    /// </summary>
    public List<int> Cards;
}