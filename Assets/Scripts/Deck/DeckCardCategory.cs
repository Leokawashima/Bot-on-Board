using System;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardCategory : MonoBehaviour
{
    [SerializeField]
    private Toggle[] m_categoryToggles;

    [SerializeField]
    private Button m_categoryAllButton;

    private Category m_categoryState = 0;

    [Flags]
    private enum Category
    {
        MeleeWeapon = 1 << 0,
        LongRangeWeapon = 1 << 1,
        Item = 1 << 2,
        Trap = 1 << 3,
        Wall = 1 << 4,
    }

    private readonly Color m_CHANGE_COLOR = new Color(0.5f, 0.5f, 0.5f, 0.0f);

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var _categoryBit = (Category[])Enum.GetValues(typeof(Category));

        for(int i = 0; i < m_categoryToggles.Length; ++i)
        {
            m_categoryState |= _categoryBit[i];

            // Toggleにイベントを設定する
            var _toggle = m_categoryToggles[i];
            var _bit = _categoryBit[i];
            _toggle.onValueChanged.AddListener((isOn_) =>
            {
                m_categoryState ^= _bit;
                var _colorBlock = _toggle.colors;
                _colorBlock.normalColor = isOn_ ?
                _colorBlock.normalColor + m_CHANGE_COLOR :
                _colorBlock.normalColor - m_CHANGE_COLOR;
                _colorBlock.selectedColor = _colorBlock.normalColor;
                _toggle.colors = _colorBlock;
            });
        }

        var _categoryAll = m_categoryState;
        m_categoryAllButton.onClick.AddListener(() =>
        {
            for (int i = 0; i < m_categoryToggles.Length; ++i)
                m_categoryToggles[i].isOn = true;
        });
    }
}