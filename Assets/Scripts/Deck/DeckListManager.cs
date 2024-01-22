using System;
using UnityEngine;
using UnityEngine.UI;
using Deck.Edit;

namespace Deck.List
{
    public class DeckListManager : SingletonMonoBehaviour<DeckListManager>
    {
        [SerializeField] private Canvas m_canvas;

        public InfoDeckData SelectDeck { get; private set; }

        [SerializeField] private DeckListInfo m_deckListInfo;
        [SerializeField] private InfoDeckDataManager m_infoManager;

        public event Action<InfoDeckData>
            Event_Edit,
            Event_Delete;

        public static void Enable() => Singleton.m_canvas.enabled = true;
        public static void Disable() => Singleton.m_canvas.enabled = false;

        public void Initialize()
        {
            DeckEditManager.Singleton.Event_Save += OnButtonSave;
            m_infoManager.Event_ClickInfo += OnClickInfo;

            m_infoManager.Initialize();
        }

        public void OnButtonListBack()
        {
            Initiate.Fade(Name.Scene.GameMode, Name.Scene.Deck, Color.black, 1.0f);
        }

        public void OnButtonEdit()
        {
            if (SelectDeck != null)
            {
                PopupDialog.Enable($"{SelectDeck.Data.Name}\nを編集します\nよろしいですか？", OnDialogEditAccept);
            }
        }
        private void OnDialogEditAccept()
        {
            Event_Edit?.Invoke(SelectDeck);
            Disable();
            DeckEditManager.Enable();
        }

        public void OnButtonDelete()
        {
            if (SelectDeck != null)
            {
                PopupDialog.Enable($"{SelectDeck.Data.Name}\nを削除します\nよろしいですか？", OnDialogDeleteAccept);
            }
        }
        private void OnDialogDeleteAccept()
        {
            DeckJsonFileSystem.DeleteJson(SelectDeck.Index);
            Event_Delete?.Invoke(SelectDeck);
            m_deckListInfo.SetInfo(SelectDeck);
        }

        private void OnClickInfo(InfoDeckData info_)
        {
            SelectDeck = info_;
            m_deckListInfo.SetInfo(info_);
        }

        private void OnButtonSave(DeckData deck_)
        {
            DeckJsonFileSystem.SaveJson(SelectDeck.Index, deck_);
            SelectDeck.SetData(deck_);
            m_deckListInfo.SetInfo(SelectDeck);

            Enable();
        }
    }
}