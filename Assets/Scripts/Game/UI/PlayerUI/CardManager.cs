using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Deck;

public class CardManager : MonoBehaviour
{
    private const int
        HAND_SIZE = 4,
        DRAW_SIZE = 2;

    [SerializeField] private MapObjectCard m_prefab;

    public MapObjectCard SelectCard { get; private set; }

    [SerializeField] private InfoCard m_info;

#if UNITY_EDITOR
    [field: SerializeField]
#endif
    public List<int> TrashCards { get; private set; } = new();

#if UNITY_EDITOR
    [field: SerializeField]
#endif
    public List<int> HandCards { get; private set; }

#if UNITY_EDITOR
    [field: SerializeField]
#endif
    public List<int> StockCards { get; private set; } = new();

    public void Initialize(DeckData deck_)
    {
        HandCards = new(HAND_SIZE);
        // 元データのリストコピーのため元データを改変しない
        var _deck = deck_.Cards.ToList();
        for (int i = 0; i < HAND_SIZE; ++i)
        {
            var _index = Random.Range(0, _deck.Count - 1);

            Draw(_deck, HandCards, _index);
        }
        StockCards = new(_deck);

        m_info.Initialize();
    }

    private void CardCreate(int index_)
    {
        var _moc = Instantiate(m_prefab, transform);
        _moc.Initialize(index_);

        var _rect = _moc.transform as RectTransform;
        _rect.localScale = Vector2.one * 0.6f;

        _moc.Event_Select += OnClick;
        _moc.Event_Trash += OnTrash;
        _moc.Event_Info += OnInfo;

        void OnClick()
        {
            if (SelectCard == _moc)
            {
                _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, -200);
                SelectCard = null;
            }
            else
            {
                if (SelectCard != null)
                {
                    var _rect = SelectCard.transform as RectTransform;
                    _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, -200);
                }
                SelectCard = _moc;
                _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, -150);
            }
        }
        void OnTrash()
        {
            TrashCards.Add(index_);
            HandCards.Remove(index_);
        }
        void OnInfo(MapObjectCard card_)
        {
            m_info.Enable();
            m_info.SetInfo(card_);
        }
    }

    public void Replenish()
    {
        var _space = HAND_SIZE - HandCards.Count;
        var _draw = Mathf.Min(_space, DRAW_SIZE);

        for (int i = 0; i < _draw; ++i)
        {
            var _index = Random.Range(0, StockCards.Count - 1);

            Draw(StockCards, HandCards, _index);
        }

        // ドローが終わったら破棄カードたちを山札に戻す
        foreach (var index in TrashCards)
        {
            StockCards.Add(index);
        }
        TrashCards.Clear();
    }

    private void Draw(List<int> from_, List<int> to_, int index_)
    {
        to_.Add(from_[index_]);

        CardCreate(from_[index_]);

        from_.RemoveAt(index_);
    }
}