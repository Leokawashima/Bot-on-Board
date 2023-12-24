using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deck
{
    /// <summary>
    /// Deck単位の情報を表示、保持するクラス
    /// </summary>
    public class InfoDeckData : MonoBehaviour
    {
        #region Field
        /// <summary>
        /// デッキのインデックスを各自保持するためのフィールド
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// デッキデータを保持するフィールド
        /// </summary>
        public DeckData Data { get; private set; }

        /// <summary>
        /// 押されたことを検知するButton
        /// </summary>
        [SerializeField] private Button m_button;

        public event Action<InfoDeckData> Event_ButtonClick;

        #region GUI

        /// <summary>
        /// 名前を表示するText
        /// </summary>
        [SerializeField] private TMP_Text m_nameText;

        #endregion GUI

        #endregion Field

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize(int index_, DeckData deck_)
        {
            Index = index_;
            Data = deck_;

            m_button.onClick.AddListener(OnButtonClick);

            ReFresh();
        }

        /// <summary>
        /// デッキデータを設定するメソッド
        /// </summary>
        /// <param name="data_">設定するデータ</param>
        public void SetData(DeckData data_)
        {
            Data = data_;
            ReFresh();
        }

        /// <summary>
        /// ボタンを押された時にイベントを呼び出すメソッド
        /// </summary>
        private void OnButtonClick()
        {
            Event_ButtonClick?.Invoke(this);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void ReFresh()
        {
            // 表示項目設定
            m_nameText.text = Data.Name;
        }
    }
}