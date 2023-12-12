using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardAppearance : MonoBehaviour
{
    public Image m_backGround;
    public Image m_frame;
    public Image m_title;
    public Image m_categoryIcon;
    public TMP_Text m_text;

    public void Copy(CardAppearance from_)
    {
        m_text.text = from_.m_text.text;

        m_backGround.color = from_.m_backGround.color;
        m_backGround.sprite = from_.m_backGround.sprite;
        m_backGround.material = from_.m_backGround.material;

        m_title.color = from_.m_title.color;
        m_title.sprite = from_.m_title.sprite;
        m_backGround.material = from_.m_title.material;

        m_frame.color = from_.m_frame.color;
        m_frame.sprite = from_.m_frame.sprite;
        m_frame.material = from_.m_frame.material;

        m_categoryIcon.color = from_.m_categoryIcon.color;
        m_categoryIcon.sprite = from_.m_categoryIcon.sprite;
        m_categoryIcon.material = from_.m_categoryIcon.material;
    }
}