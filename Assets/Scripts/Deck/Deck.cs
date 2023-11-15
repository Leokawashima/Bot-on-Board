using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public enum State
    {
        isRarilyLimit = 1 << 0,
        isSameBan = 1 << 1,
    }

    /// <summary>
    /// デッキのフィールド
    /// </summary>
    [field: SerializeField]
    public List<int> CardIndexList { get; private set; } = new();

    /// <summary>
    /// デッキのサイズ
    /// </summary>
    public int MaxSize { get; private set; } = 10;

    /// <summary>
    /// デッキの状態
    /// </summary>
    public int HasState { get; private set; }
}