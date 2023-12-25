using Map;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Deck.Edit
{
    public class DeckEditCategory : MonoBehaviour
    {
        [SerializeField] private Toggle[] m_categoryToggles;

        [SerializeField] private Button m_categoryAllButton;

        private int m_categoryBitState = 0;

        private readonly Color m_CHANGE_COLOR = new(0.5f, 0.5f, 0.5f, 0.0f);

        public void Initialize()
        {
            for (int i = 0; i < m_categoryToggles.Length; ++i)
            {
                var _bit = 1 << i;
                m_categoryBitState |= _bit;

                // Toggleにイベントを設定する
                var _toggle = m_categoryToggles[i];
                _toggle.onValueChanged.AddListener((isOn_) =>
                {
                    m_categoryBitState ^= _bit;
                    var _colorBlock = _toggle.colors;
                    _colorBlock.normalColor = isOn_ ?
                    _colorBlock.normalColor + m_CHANGE_COLOR :
                    _colorBlock.normalColor - m_CHANGE_COLOR;
                    _colorBlock.selectedColor = _colorBlock.normalColor;
                    _toggle.colors = _colorBlock;
                });
            }

            var _categoryAll = m_categoryBitState;
            m_categoryAllButton.onClick.AddListener(() =>
            {
                foreach (var toggle in m_categoryToggles)
                {
                    toggle.isOn = true;
                }
            });
        }
    }
}