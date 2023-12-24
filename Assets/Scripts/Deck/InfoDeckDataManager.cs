using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deck
{
    public class InfoDeckDataManager : MonoBehaviour
    {
        [SerializeField] private RectTransform m_content;
        [SerializeField] private InfoDeckData m_prefab;

        private const int DECK_PRESET_SIZE = 10;
        private const string DECK_NAME = "Deck";

        // デフォルトのデータをいれるためだけのSOをいれている
        [SerializeField] private DeckData_SO m_deckDefault;

        public List<InfoDeckData> Infos = new();

        public event Action<InfoDeckData> Event_ClickInfo;

        public void Initialize()
        {
            OpenDeckList();
            DeckListManager.Singleton.Event_Delete += OnDelete;
        }

        private void OpenDeckList()
        {
            // 最初のデッキデータは空になってほしくない為にデータがない場合既存のデータから読み取る
            {
                if (OpenDeck(0, out var _deck, out var _info))
                {
                    _info.Initialize(0, _deck);
                }
                else
                {
                    _info.Initialize(0, m_deckDefault.Deck.DeepCopyInstance());
                }
                _info.Event_ButtonClick += Event_ClickInfo;
                Infos.Add(_info);
            }

            // 2～10は空でも良いため空のデータにする
            for (int i = 1; i < DECK_PRESET_SIZE; ++i)
            {
                if (OpenDeck(i, out var _deck, out var _info))
                {
                    _info.Initialize(i, _deck);
                }
                else
                {
                    _info.Initialize(i, new()
                    {
                        Name = DECK_NAME + (i + 1)
                    });
                }
                _info.Event_ButtonClick += Event_ClickInfo;
                Infos.Add(_info);
            }
        }

        private void OnDelete(InfoDeckData info_)
        {
            if (info_.Index == 0)
            {
                info_.Initialize(0, m_deckDefault.Deck.DeepCopyInstance());
            }
            else
            {
                info_.Initialize(info_.Index, new()
                {
                    Name = DECK_NAME + (info_.Index + 1)
                });
            }
        }

        private bool OpenDeck(int index_, out DeckData deck_, out InfoDeckData info_)
        {
            info_ = Instantiate(m_prefab, m_content);

            return DeckJsonFileSystem.LoadJson(index_, out deck_);
        }
    }
}