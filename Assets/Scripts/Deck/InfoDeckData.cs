using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public void Initialize(int index_, DeckData deck_, Action<InfoDeckData> action_)
    {
        // 最低必要情報設定
        Index = index_;
        Data = deck_;

        // メソッド登録
        m_button.onClick.AddListener(() =>
        {
            action_(this);
        });

        ReFresh();
    }
    
    /// <summary>
    /// 更新処理
    /// </summary>
    public void ReFresh()
    {
        // 表示項目設定
        m_nameText.text = Data.Name;
    }
}