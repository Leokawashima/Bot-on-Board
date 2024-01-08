using UnityEngine;

public abstract class Rarity_Template : ScriptableObject
{
    public abstract void Generate(CardAppearance appearance_, Category_SO category_);
}