using System;
using System.Collections.Generic;

namespace Deck
{
    /// <summary>
    /// デッキ当たりのデータクラス
    /// </summary>
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
        /// デッキのランク毎のカウント
        /// </summary>
        public int[] Rarity;

        /// <summary>
        /// デッキのカテゴリ毎のカウント
        /// </summary>
        public int[] Category;

        /// <summary>
        /// デッキのフィールド
        /// </summary>
        public List<int> Cards = new();
    }
}