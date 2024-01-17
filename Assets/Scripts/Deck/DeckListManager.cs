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

        #region ベータ用に最低限機能を実装した部分
        [SerializeField] private Button m_backButton;
        [SerializeField] private RectTransform
            m_playerFirst,
            m_playerSecond;
        [SerializeField] private Button
            m_playerFirstSetButton,
            m_playerSecondSetButton;

        #endregion ベータ用に最低限機能を実装した部分

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

            m_playerFirst.gameObject.SetActive(false);
            m_playerSecond.gameObject.SetActive(false);
            m_playerFirstSetButton.onClick.AddListener(OnButtonPlayerFirst);
            m_playerSecondSetButton.onClick.AddListener(OnButtonPlayerSecond);

            m_infoManager.Initialize();
            if (GlobalSystem.IndexPlayerFirst != -1)
            {
                var _index = GlobalSystem.IndexPlayerFirst;
                m_playerFirst.gameObject.SetActive(true);
                m_playerFirst.SetParent(m_infoManager.Infos[_index].transform);
                m_playerFirst.transform.localPosition = new Vector3(350, 30, 0);
            }
            if (GlobalSystem.IndexPlayerSecond != -1)
            {
                var _index = GlobalSystem.IndexPlayerSecond;
                m_playerSecond.gameObject.SetActive (true);
                m_playerSecond.SetParent(m_infoManager.Infos[_index].transform);
                m_playerSecond.transform.localPosition = new Vector3(400, 30, 0);
            }
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

        private void OnButtonPlayerFirst()
        {
            m_playerFirst.gameObject.SetActive(true);
            m_playerFirst.SetParent(SelectInfo.transform);
            m_playerFirst.transform.localPosition = new Vector3(350, 30, 0);
            GlobalSystem.IndexPlayerFirst = SelectInfo.Index;
            GlobalSystem.DeckPlayerFirst = SelectInfo.Data.DeepCopyInstance();
        }
        private void OnButtonPlayerSecond()
        {
            m_playerSecond.gameObject.SetActive(true);
            m_playerSecond.SetParent(SelectInfo.transform);
            m_playerSecond.transform.localPosition = new Vector3(400, 30, 0);
            GlobalSystem.IndexPlayerSecond = SelectInfo.Index;
            GlobalSystem.DeckPlayerSecond = SelectInfo.Data.DeepCopyInstance();
        }
    }
}