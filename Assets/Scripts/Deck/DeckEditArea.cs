using UnityEngine;

public class DeckEditArea : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_baseRectTransform;

    [SerializeField]
    private MapObjectCard[] m_editCardList;

    public bool CheckHitCard(Vector2 mousePos_, out MapObjectCard card_)
    {
        if (CheckHit(m_baseRectTransform, mousePos_))
        {
            foreach (var card in m_editCardList)
            {
                var _rect = card.transform as RectTransform;
                if (CheckHit(_rect, mousePos_))
                {
                    card_ = card;
                    return true;
                }
            }
        }
        card_ = null;
        return false;
    }

    private bool CheckHit(RectTransform rect_, Vector2 mousePos_)
    {
        return CheckHit(rect_.anchoredPosition, rect_.sizeDelta, mousePos_);
    }

    private bool CheckHit(Vector2 pos_, Vector2 size_, Vector2 mousePos_)
    {
        if (pos_.x - size_.x / 2.0f < mousePos_.x && pos_.x + size_.x / 2.0f > mousePos_.x)
        {
            if (pos_.y - size_.y / 2.0f < mousePos_.y && pos_.y + size_.y / 2.0f > mousePos_.y)
            {
                return true;
            }
            return false;
        }
        return false;
    }
}