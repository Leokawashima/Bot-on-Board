﻿using UnityEngine;
using Map;

public class CardGenerator : MonoBehaviour
{
    [SerializeField] CardImageTable_SO m_cardTable;

    private readonly float[] m_CATEGORY_COLOR = new float[]
    {
        0.0f,
        240.0f/ 360.0f,
        120.0f/ 360.0f,
        300.0f/ 360.0f,
        30.0f/ 360.0f,
    };

    public MapObjectCard Create(int index_, Transform parent_)
    {
        var _moc = Instantiate(m_cardTable.Prefab, parent_);
        _moc.Initialize(index_);

        var _appearance = _moc.GetComponent<CardAppearance>();

        var _rarity = _moc.SO.HasRarity;
        var _category = (int)_moc.SO.HasCategory;

        _appearance.m_categoryIcon.sprite = m_cardTable.Icon[_category];
        _appearance.m_text.text = _moc.SO.Name;
        _appearance.m_title.sprite = _moc.SO.TitleImage;

        switch(_rarity)
        {
            case Rarity.Common:
                {
                    _appearance.m_backGround.color = Color.HSVToRGB(m_CATEGORY_COLOR[_category], 0.2f, 1.0f);
                    _appearance.m_frame.sprite = m_cardTable.Frame_Base;
                    _appearance.m_frame.color = Color.HSVToRGB(m_CATEGORY_COLOR[_category], 0.5f, 1.0f);
                }
                break;
            case Rarity.UnCommon:
                {
                    _appearance.m_backGround.sprite = m_cardTable.BG_UnCommon;
                    _appearance.m_frame.sprite = m_cardTable.Frame_Base;
                    _appearance.m_frame.color = Color.HSVToRGB(m_CATEGORY_COLOR[_category], 0.5f, 1.0f);
                }
                break;
            case Rarity.Rare:
                {
                    _appearance.m_backGround.sprite = m_cardTable.BG_Rare;
                    _appearance.m_frame.sprite = m_cardTable.Frame_Base;
                    _appearance.m_frame.color = Color.HSVToRGB(m_CATEGORY_COLOR[_category], 0.5f, 1.0f);
                }
                break;
            case Rarity.Epic:
                {
                    _appearance.m_backGround.sprite = m_cardTable.BG_Epic[_category];

                    var _color = _appearance.m_categoryIcon.color;
                    _color.a = 0.2f;
                    _appearance.m_categoryIcon.color = _color;

                    _appearance.m_frame.sprite = m_cardTable.Frame_Epic;

                    _appearance.m_text.color = Color.white;
                }
                break;
        }

        return _moc;
    }
}