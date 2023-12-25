using System;
using UnityEngine;

[CreateAssetMenu(menuName = "BoB/Card/Rarity")]
public class CardRarityTable_SO : ScriptableObject
{
    public Rarity[] Table;

    [Serializable]
    public class Rarity
    {
        public string Name;
    }
}