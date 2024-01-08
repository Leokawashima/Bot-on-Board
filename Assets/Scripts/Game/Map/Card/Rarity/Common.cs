using UnityEngine;

[CreateAssetMenu(menuName = "BoB/Card/Rarity/Common")]
public class Common : Rarity_Template
{
    public override void Generate(CardAppearance appearance_, Category_SO category_)
    {
        appearance_.m_categoryIcon.sprite = category_.Icon;
        appearance_.m_backGround.color = category_.Color_BG;
        appearance_.m_frame.color = category_.Color_Frame;
    }
}