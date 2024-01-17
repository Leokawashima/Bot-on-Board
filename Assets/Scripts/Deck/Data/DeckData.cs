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
        public string Name = "ナナシ";

        /// <summary>
        /// デッキの状態
        /// </summary>
        public DeckManager.DeckState State;

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
        public List<int> Cards;

        public DeckData()
        {
            // データベースからひぱってくるコードを書く　今は仮
            Rarity = new int[4];
            Category = new int[5];
            Cards = new();
        }
        public DeckData(string name_)
        {
            // データベースからひぱってくるコードを書く　今は仮
            Name = name_;
            Rarity = new int[4];
            Category = new int[5];
            Cards = new();
        }
    }
}