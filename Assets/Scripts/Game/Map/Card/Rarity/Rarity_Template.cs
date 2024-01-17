using UnityEngine;

public abstract class Rarity_Template : ScriptableObject
{
    [field: SerializeField] public int ID { get; protected set; } = 0;
    [field: SerializeField] public Sprite Icon { get; private set; }
    public abstract void Generate(CardAppearance appearance_, Category_SO category_);
}