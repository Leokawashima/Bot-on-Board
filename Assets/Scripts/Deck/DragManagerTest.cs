using UnityEngine;

public class DragManagerTest : MonoBehaviour
{
    [SerializeField] DragTest[] cards;
    [SerializeField] MapObjectCard m_selectionCard;
    [SerializeField] MapObjectCard m_targetCard;

    private RectTransform m_selectionRectTransform;
    private RectTransform m_targetRectTransform;

    void Start()
    {
        foreach(var card in cards)
        {
            card.OnBeginDrag += OnBeginDrag;
            card.OnDrag += OnDrag;
            card.OnEndDrag += OnEndDrag;
        }

        m_selectionCard.gameObject.SetActive(false);
        m_selectionRectTransform = m_selectionCard.transform as RectTransform;

        m_targetRectTransform = m_targetCard.transform as RectTransform;
    }

    private void OnBeginDrag(Vector2 mousePos_, MapObjectCard card_)
    {
        m_selectionCard.gameObject.SetActive(true);
        m_selectionRectTransform.localPosition = mousePos_;
        //m_selectionCard = card_;
    }

    private void OnDrag(Vector2 mousePos_, MapObjectCard card_)
    {
        var _isHit = false;

        var _pos = m_targetRectTransform.anchoredPosition;
        var _size = m_targetRectTransform.sizeDelta;
        if(_pos.x - _size.x / 2.0f < mousePos_.x && _pos.x + _size.x / 2.0f > mousePos_.x)
        {
            if(_pos.y - _size.y / 2.0f < mousePos_.y && _pos.y + _size.y / 2.0f > mousePos_.y)
            {
                _isHit = true;
            }
        }

        if(_isHit)
        {
            m_selectionRectTransform.localPosition = m_targetRectTransform.anchoredPosition;
            m_selectionRectTransform.sizeDelta = m_targetRectTransform.sizeDelta;
        }
        else
        {
            m_selectionRectTransform.localPosition = mousePos_;
            m_selectionRectTransform.sizeDelta = new Vector2(100, 100);
        }
    }

    private void OnEndDrag(Vector2 mousePos_, MapObjectCard card_)
    {
        m_selectionCard.gameObject.SetActive(false);

        var _isHit = false;

        var _pos = m_targetRectTransform.anchoredPosition;
        var _size = m_targetRectTransform.sizeDelta;
        if(_pos.x - _size.x / 2.0f < mousePos_.x && _pos.x + _size.x / 2.0f > mousePos_.x)
        {
            if(_pos.y - _size.y / 2.0f < mousePos_.y && _pos.y + _size.y / 2.0f > mousePos_.y)
            {
                _isHit = true;
            }
        }

        if(_isHit)
        {
            //m_targetCard = m_selectionCard;
        }
        else
        {
            //m_targetCard = null;
        }
    }
}
