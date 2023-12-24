using System;
using UnityEngine;
using UnityEngine.UI;

namespace Deck
{
    public class DeckEditManager : SingletonMonoBehaviour<DeckEditManager>
    {
        [SerializeField] Canvas m_canvas;

        [SerializeField] DeckEditArea m_editArea;
        [SerializeField] DeckListInfo m_listInfo;
        [SerializeField] DeckInfoArea m_infoArea;

        [SerializeField] private Button
            m_backButton,
            m_saveButton;
        [SerializeField] private PopupDialog
            m_backDialog,
            m_saveDialog;

        public event Action Event_Back;
        public event Action<DeckData> Event_Save;

        public void Enable()
        {
            m_canvas.enabled = true;
        }
        public void Disable()
        {
            m_canvas.enabled = false;
        }

        public void Initialize()
        {
            Disable();
            m_infoArea.Initialize();

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
                Name = m_infoArea.NameText,
                Cards = new(m_editArea.EditCards.Count),
            };
            for (int i = 0, cnt = m_editArea.EditCards.Count; i < cnt; ++i)
            {
                _deckData.Cards.Add(m_editArea.EditCards[i].Index);
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