using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoDeckData : MonoBehaviour
{
    public int Index { get; private set; }

    public DeckData Data { get; private set; }

    [SerializeField] private TMP_Text m_text;
    public string NameText { get { return m_text.text; } set { m_text.text = value; } }
    [SerializeField] private Button m_button;

    public void Initialize(int index_, DeckData deck_, Action<InfoDeckData> action_)
    {
        // 最低必要情報設定
        Index = index_;
        Data = deck_;

        // 表示項目設定
        NameText = deck_.Name;

        // メソッド登録
        m_button.onClick.AddListener(() =>
        {
            action_(this);
        });
    }
}