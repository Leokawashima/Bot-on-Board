using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoCardData : SingletonMonoBehaviour<InfoCardData>
{
    [SerializeField] private Canvas m_canvas;

    [SerializeField] private Button m_backButton;

    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private TMP_Text m_infoText;

    [SerializeField] private Image m_objectImage;
    [SerializeField] private Image m_categoryImage;
    [SerializeField] private Image m_rarityImage;

    public static void Enable(MapObjectCard card_)
    {
        Singleton.m_canvas.enabled = true;
        SetInfo(card_);

        static void SetInfo(MapObjectCard card_)
        {
            Singleton.m_nameText.text = card_.SO.Name;
            Singleton.m_infoText.text = card_.SO.Info;
            Singleton.m_objectImage.sprite = card_.SO.TitleImage;
            Singleton.m_categoryImage.sprite = card_.SO.Category.Image;
            Singleton.m_rarityImage.sprite = card_.SO.Rarity.Icon;
        }
    }
}