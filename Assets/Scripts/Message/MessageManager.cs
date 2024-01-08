using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// メッセージ表示の管理を行うクラス
/// </summary>
public class MessageManager : MonoBehaviour
{
    /// <summary>
    /// メッセージテキスト
    /// </summary>
    [SerializeField]
    private TMP_Text m_text;

    /// <summary>
    /// a値0~1までの変化にかかる時間　秒単位
    /// </summary>
    [SerializeField]
    private float m_fadeTime = 1.0f;

    /// <summary>
    /// 表示するメッセージ
    /// </summary>
    [TextArea, SerializeField]
    private List<string> m_messageList;

    /// <summary>
    /// 現在表示している文字のインデックス
    /// </summary>
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    private int m_index = 0;

    /// <summary>
    /// コルーチンのアクティブなものを保持するフィールド
    /// </summary>
    private Coroutine m_activeCorutine;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        InputManager.Event_Any += OnAny;

        /* 
         * TextAreaAttributeを付与しない場合\nで改行するために差し替え処理が必要
         * インスペクターで入れた文字列をTMP_Textで表示するために改行を差し替え
         * for (int i = 0; i < m_messageList.Count; ++i)
         * {
         *     if (m_messageList[i].Contains("\\n"))
         *         m_messageList[i] = m_messageList[i].Replace("\\n", Environment.NewLine);
         * }
         */

        // 最初の文字を反映
        m_text.text = m_messageList[m_index];

        m_activeCorutine = StartCoroutine(CoAwake());
    }

    /// <summary>
    /// InputSystemのイベントハンドラ
    /// </summary>
    private void OnAny()
    {
        SetNextMessage();
    }

    /// <summary>
    /// メッセージを次へ移動させる
    /// </summary>
    private void SetNextMessage()
    {
        // 現在のインデックスが最後の文字かどうか
        if (m_index + 1 != m_messageList.Count)
        {
            if (m_activeCorutine != null)
            {
                StopCoroutine(m_activeCorutine);
                m_text.text = m_messageList[m_index];
            }
            m_index++;
            m_activeCorutine = StartCoroutine(CoFade());
        }
        else
        {
            InputManager.Event_Any -= OnAny;

            if (m_activeCorutine != null)
            {
                StopCoroutine(m_activeCorutine);
            }
            StartCoroutine(CoSceneLoad());
        }
    }

    /// <summary>
    /// 最初の文字をフェードさせるコルーチン
    /// </summary>
    private IEnumerator CoAwake()
    {
        m_text.SetAlpha(0);

        while (m_text.color.a <= 1)
        {
            m_text.AddAlpha(Time.deltaTime / m_fadeTime);
            yield return null;
        }

        m_text.SetAlpha(1);
        m_activeCorutine = null;
    }

    /// <summary>
    /// 文字から文字へ切り替えるときに使用するコルーチン
    /// </summary>
    private IEnumerator CoFade()
    {
        while (m_text.color.a >= 0)
        {
            m_text.SubAlpha(Time.deltaTime / m_fadeTime);
            yield return null;
        }
        m_text.SetAlpha(0);

        m_text.text = m_messageList[m_index];
        yield return null;

        while (m_text.color.a <= 1)
        {
            m_text.AddAlpha(Time.deltaTime / m_fadeTime);
            yield return null;
        }
        m_text.SetAlpha(1);
        m_activeCorutine = null;
    }

    /// <summary>
    /// シーン切り替えを呼び出す最後のコルーチン
    /// </summary>
    private IEnumerator CoSceneLoad()
    {
        while (m_text.color.a >= 0)
        {
            m_text.SubAlpha(Time.deltaTime / m_fadeTime);
            yield return null;
        }

        m_text.SetAlpha(0);
        Initiate.Fade(Name.Scene.Title, Name.Scene.Message, Color.black, 1.0f);
    }
}