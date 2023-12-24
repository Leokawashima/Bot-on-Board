﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Deck;

public class CardManager : MonoBehaviour
{
    private const int
        HAND_SIZE = 4,
        DRAW_SIZE = 2;

    [SerializeField] private CardGenerator m_cardGenerator;

    public MapObjectCard GetSelectCard
    {
        get
        {
            return m_toggleGroup.ActiveToggles().FirstOrDefault()?.
                GetComponent<MapObjectCard>();
        }
    }

    [SerializeField] private ToggleGroup m_toggleGroup;

    [SerializeField] private InfoCard m_info;

#if UNITY_EDITOR
    [field: SerializeField]
#endif
    public List<int> TrashCardList { get; private set; } = new();

#if UNITY_EDITOR
    [field: SerializeField]
#endif
    public List<int> HandCardList { get; private set; }

#if UNITY_EDITOR
    [field: SerializeField]
#endif
    public List<int> StockCardList { get; private set; } = new();

    public void Initialize(DeckData deck_)
    {
        HandCardList = new(HAND_SIZE);
        // 元データのリストコピーのため元データを改変しない
        var _deck = deck_.Cards.ToList();
        for (int i = 0; i < HAND_SIZE; ++i)
        {
            var _index = Random.Range(0, _deck.Count - 1);

            HandCardList.Add(_deck[_index]);

            CardCreate(_deck[_index]);

            _deck.RemoveAt(_index);
        }
        StockCardList = new(_deck);

        m_info.Initialize();
    }

    private void CardCreate(int index_)
    {
        var _moc = m_cardGenerator.Create(index_, transform);

        var _toggle = _moc.gameObject.GetComponent<Toggle>();
        _toggle.group = m_toggleGroup;

        _moc.Event_Trash += () =>
        {
            TrashCardList.Add(index_);
            HandCardList.Remove(index_);
        };
        _moc.Event_Info += (MapObjectCard card_) =>
        {
            m_info.Enable();
            m_info.SetInfo(card_);
        };

        var _rect = _moc.transform as RectTransform;
        _rect.localScale = Vector2.one * 0.6f;

        _toggle.onValueChanged.AddListener((bool isOn_) =>
        {
            _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, isOn_ ? -150 : -200);
        });
    }
    
    public void Draw()
    {
        var _space = HAND_SIZE - HandCardList.Count;
        var _draw = Mathf.Min(_space, DRAW_SIZE);

        for (int i = 0; i < _draw; ++i)
        {
            var _index = Random.Range(0, StockCardList.Count - 1);
            HandCardList.Add(StockCardList[_index]);

            CardCreate(StockCardList[_index]);

            StockCardList.RemoveAt(_index);
        }

        // ドローが終わったら破棄カードたちを山札に戻す
        foreach (var index in TrashCardList)
        {
            StockCardList.Add(index);
        }
        TrashCardList.Clear();
    }
}