using System;
using UnityEngine;
using UnityEngine.UI;
using Deck.Edit;

namespace Deck.List
{
    public class DeckListManager : SingletonMonoBehaviour<DeckListManager>
    {
        [SerializeField] Canvas m_canvas;

        public InfoDeckData SelectInfo { get; private set; }

        [SerializeField] DeckListInfo m_deckListInfo;
        [SerializeField] InfoDeckDataManager m_infoManager;

        [SerializeField] private Button
            m_editButton,
            m_deleteButton;

        [SerializeField] private Button m_backButton;

        public event Action<InfoDeckData>
            Event_Edit,
            Event_Delete;

        private void Enable() => m_canvas.enabled = true;
        private void Disable() => m_canvas.enabled = false;

        public void Initialize()
        {
            Enable();

            DeckEditManager.Singleton.Event_Back += OnButtonBack;
            DeckEditManager.Singleton.Event_Save += OnButtonSave;
            m_infoManager.Event_ClickInfo += OnClickInfo;

            m_backButton.onClick.AddListener(OnButtonListBack);

            m_editButton.onClick.AddListener(OnButtonEdit);
            m_deleteButton.onClick.AddListener(OnButtonDelete);

            m_infoManager.Initialize();
        }

        private void OnButtonListBack()
        {
            Initiate.Fade(Name.Scene.GameMode, Name.Scene.Deck, Color.black, 1.0f);
        }

        private void OnButtonEdit()
        {
            if (SelectInfo != null)
            {
                PopupDialog.Enable($"{SelectInfo.Data.Name}\nを編集します\nよろしいですか？", OnDialogEditAccept);
            }
        }
        private void OnDialogEditAccept()
        {
            Disable();
            Event_Edit?.Invoke(SelectInfo);
        }

        private void OnButtonDelete()
        {
            if (SelectInfo != null)
            {
                PopupDialog.Enable($"{SelectInfo.Data.Name}\nを削除します\nよろしいですか？", OnDialogDeleteAccept);
            }
        }
        private void OnDialogDeleteAccept()
        {
            DeckJsonFileSystem.DeleteJson(SelectInfo.Index);
            Event_Delete?.Invoke(SelectInfo);
            m_deckListInfo.SetInfo(SelectInfo);
        }

        private void OnClickInfo(InfoDeckData info_)
        {
            SelectInfo = info_;
            m_deckListInfo.SetInfo(info_);
        }

        private void OnButtonBack()
        {
            Enable();
        }
        private void OnButtonSave(DeckData deck_)
        {
            DeckJsonFileSystem.SaveJson(SelectInfo.Index, deck_);
            SelectInfo.SetData(deck_);
            m_deckListInfo.SetInfo(SelectInfo);

            Enable();
        }
    }
}