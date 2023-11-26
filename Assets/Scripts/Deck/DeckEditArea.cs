using UnityEngine;

public class DeckEditArea : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_baseRectTransform;

    private MapObjectCard[] m_editCardList;

    [SerializeField]
    private MapObjectCard m_targetCard;

    [SerializeField] private Vector2 m_position = new Vector2(620, 108);
    [SerializeField] private Vector2 m_offset = new Vector2(200, -134);
    [SerializeField] private int m_sheat = 2;
    [SerializeField] private int m_deckSize = 10;

    private void Start()
    {
        m_editCardList = new MapObjectCard[m_deckSize];
        for(int i = 0; i < m_deckSize; ++i)
        {
            CardCreate(i);
        }
    }

    private void CardCreate(int index_)
    {
        var _moc = Instantiate(m_targetCard, transform);
        m_editCardList[index_] = _moc;
        var _rect = _moc.transform as RectTransform;
        _rect.anchoredPosition = m_position + new Vector2(m_offset.x * (index_ % m_sheat), m_offset.y * (int)(index_ / m_sheat));
    }

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