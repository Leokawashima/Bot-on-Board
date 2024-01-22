using System;
using UnityEngine;
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

        public event Action Event_Back;
        public event Action<DeckData> Event_Save;

        public static void Enable() => Singleton.m_canvas.enabled = true;
        public static void Disable() => Singleton.m_canvas.enabled = false;

        public void Initialize()
        {
            DragManager.Initialize();
            Cards.Initialize();
            Category.Initialize();
            Info.Initialize();
            Setting.Initialize();
            Search.Initialize();
        }

        public void OnButtonBack()
        {
            PopupDialog.Enable("変更せずに戻ります\nよろしいですか？", OnDialogBackAccept);
        }
        private void OnDialogBackAccept()
        {
            Event_Back?.Invoke();
            Disable();
        }

        public void OnButtonSave()
        {
            PopupDialog.Enable("保存して終了します\nよろしいですか？", OnDialogSaveAccept);
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
            DeckListManager.Enable();
        }
    }
}