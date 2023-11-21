using System.Collections.Generic;

public class DeckData
{
    /// <summary>
    /// デッキの名前
    /// </summary>
    public string Name = "null";

    /// <summary>
    /// デッキの状態
    /// </summary>
    public DeckEditingManager.DeckState State = 0;

    /// <summary>
    /// デッキのフィールド
    /// </summary>
    public List<int> CardIndexList = new();

    /// <summary>
    /// デッキのサイズ
    /// </summary>
    public int MaxSize = 10;
}