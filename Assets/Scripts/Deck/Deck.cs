using System;
using System.Collections.Generic;

public class Deck
{
    /// <summary>
    /// デッキの名前
    /// </summary>
    public string DeckName = "null";

    /// <summary>
    /// デッキの状態
    /// </summary>
    public int State = 0;

    /// <summary>
    /// デッキのフィールド
    /// </summary>
    public List<int> CardIndexList = new();

    /// <summary>
    /// デッキのサイズ
    /// </summary>
    public int MaxSize = 10;
}