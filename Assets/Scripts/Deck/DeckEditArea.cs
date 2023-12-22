using System.Collections.Generic;
using UnityEngine;

public class DeckEditArea : MonoBehaviour
{
    private RectTransform m_rectTransform;

    public List<MapObjectCard> EditCards { get; private set; }

    [SerializeField] private MapObjectCard m_targetCard;

    [SerializeField] private int m_defaultDeckSize = 10;

    private void Start()
    {
        m_rectTransform = transform as RectTransform;

        EditCards = new(m_defaultDeckSize);
        for(int i = 0, cnt = m_defaultDeckSize; i < cnt; ++i)
        {
            CardCreate();
        }
    }

    private void CardCreate()
    {
        var _moc = Instantiate(m_targetCard, m_rectTransform);
        EditCards.Add(_moc);
    }

    public bool CheckHitCard(Vector2 mousePos_, out MapObjectCard card_)
    {
        if (CheckHit(m_rectTransform, mousePos_))
        {
            foreach (var card in EditCards)
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
        var _pos = rect_.position;
        var _size = rect_.sizeDelta * rect_.localScale;
        var _sizeHalf = _size / 2.0f;

        return _InSide(_pos.x, _sizeHalf.x, mousePos_.x) && _InSide(_pos.y, _sizeHalf.y, mousePos_.y);
        
        bool _InSide(float pos_, float sizeHalf_, float mousePos_)
        {
            return pos_ - sizeHalf_ < mousePos_ && pos_ + sizeHalf_ > mousePos_;
        }
    }
}