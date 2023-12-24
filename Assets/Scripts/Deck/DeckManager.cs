using System;
using UnityEngine;

namespace Deck
{
    public class DeckManager : SingletonMonoBehaviour<DeckManager>
    {
        [SerializeField] private DeckListManager m_deckList;
        [SerializeField] private DeckEditManager m_deckEdit;

        [Flags]
        public enum DeckState
        {
            Non = 0,
            isRarilyLimit = 1 << 0,
            isSameBan = 1 << 1,
        }

        private void Start()
        {
            m_deckList.Initialize();
            m_deckEdit.Initialize();
        }
    }
}