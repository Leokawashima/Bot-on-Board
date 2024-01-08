using UnityEngine;

[CreateAssetMenu(menuName = "BoB/Card/Rarity/Epic")]
public class Epic : Rarity_Template
{
    [SerializeField] private Sprite m_frame;
    [SerializeField] private Sprite[] m_backGround = new Sprite[5];

    public override void Generate(CardAppearance appearance_, Category_SO category_)
    {
        switch (category_.Name)
        {
            case "MelleWeapon":
                appearance_.m_backGround.sprite = m_backGround[0];
                break;
            case "LongRangeWeapon":
                appearance_.m_backGround.sprite = m_backGround[1];
                break;
            case "Item":
                appearance_.m_backGround.sprite = m_backGround[2];
                break;
            case "Trap":
                appearance_.m_backGround.sprite = m_backGround[3];
                break;
            case "Wall":
                appearance_.m_backGround.sprite = m_backGround[4];
                break;
        }

        appearance_.m_categoryIcon.color = category_.Color_Icon;

        appearance_.m_frame.sprite = m_frame;

        appearance_.m_text.color = category_.Color_Text;
    }
}