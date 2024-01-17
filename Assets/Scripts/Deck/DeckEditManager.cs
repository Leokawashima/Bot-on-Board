using System;
using UnityEngine;
using UnityEngine.UI;
using Deck.List;

namespace Deck.Edit
{
    public class DeckEditManager : SingletonMonoBehaviour<DeckEditManager>
    {
        [SerializeField] Canvas m_canvas;

        [field: SerializeField] public DeckEditDragManager DragManager { get; private set; }
        [field: SerializeField] public DeckEditCards Cards { get; private set; }
        [field: SerializeField] public DeckEditCategory Category { get; private set; }
        [field: SerializeField] public DeckEditInfo Info { get; private set; }
        [field: SerializeField] public DeckEditSetting Setting { get; private set; }
        [field: SerializeField] public DeckEditSearch Search { get; private set; }

        [SerializeField] private Button
            m_backButton,
            m_saveButton;
        [SerializeField] private PopupDialog
            m_backDialog,
            m_saveDialog;

        public event Action Event_Back;
        public event Action<DeckData> Event_Save;

        public void Enable() => m_canvas.enabled = true;
        public void Disable() => m_canvas.enabled = false;

        public void Initialize()
        {
            Disable();
            DragManager.Initialize();
            Cards.Initialize();
            Category.Initialize();
            Info.Initialize();
            Setting.Initialize();
            Search.Initialize();

            DeckListManager.Singleton.Event_Edit += (InfoDeckData deta_) =>
            {
                Enable();
            };

            m_backButton.onClick.AddListener(OnButtonBack);
            m_backDialog.AcceptButton.onClick.AddListener(OnDialogBackAccept);
            m_backDialog.CancelButton.onClick.AddListener(OnDialogBackCancel);

            m_saveButton.onClick.AddListener(OnButtonSave);
            m_saveDialog.AcceptButton.onClick.AddListener(OnDialogSaveAccept);
            m_saveDialog.CancelButton.onClick.AddListener(OnDialogSaveCancel);
        }

        private void OnButtonBack()
        {
            m_backDialog.DialogText.text = "変更せずに戻ります\nよろしいですか？";
            m_backDialog.Enable();
        }
        private void OnDialogBackAccept()
        {
            Disable();
            Event_Back?.Invoke();

            m_backDialog.Disable();
        }
        private void OnDialogBackCancel()
        {
            m_backDialog.Disable();
        }

        private void OnButtonSave()
        {
            m_saveDialog.DialogText.text = "保存して終了します\nよろしいですか？";
            m_saveDialog.Enable();
        }
        private void OnDialogSaveAccept()
        {
            var _deckData = new DeckData()
            {
                Name = Info.NameText,
                Cards = new(DragManager.EditCards.Count),
            };
            for (int i = 0, cnt = DragManager.EditCards.Count; i < cnt; ++i)
            {
                _deckData.Cards.Add(DragManager.EditCards[i].Index);
            }
            Event_Save?.Invoke(_deckData);

            Disable();
            m_saveDialog.Disable();
        }
        private void OnDialogSaveCancel()
        {
            m_saveDialog.Disable();
        }
    }
}