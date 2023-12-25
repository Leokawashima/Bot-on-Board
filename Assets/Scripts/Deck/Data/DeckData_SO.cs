using UnityEngine;

namespace Deck
{
    /// <summary>
    /// デッキデータをSOにしたクラス
    /// </summary>
    [CreateAssetMenu(fileName = "DeckData", menuName = "BoB/Deck/DeckData")]
    public class DeckData_SO : ScriptableObject
    {
        public DeckData Deck;
    }
}