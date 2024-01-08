using UnityEngine;

[CreateAssetMenu(menuName = "BoB/Card/Rarity/UnCommon")]
public class UnCommon : Rarity_Template
{
    [SerializeField] private Sprite m_backGround;

    public override void Generate(CardAppearance appearance_, Category_SO category_)
    {
        appearance_.m_categoryIcon.sprite = category_.Icon;
        appearance_.m_backGround.sprite = m_backGround;
        appearance_.m_frame.color = category_.Color_Frame;
    }
}