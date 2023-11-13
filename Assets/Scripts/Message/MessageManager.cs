using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// メッセージ表示の管理を行うクラス
/// </summary>
public class MessageManager : MonoBehaviour
{
    /// <summary>
    /// メッセージテキスト
    /// </summary>
    [SerializeField] TMP_Text m_text;

    /// <summary>
    /// a値0~1までの変化にかかる時間　秒単位
    /// </summary>
    [SerializeField] float m_fadeTime = 1.0f;

    /// <summary>
    /// 表示するメッセージ
    /// </summary>
    [TextArea, SerializeField] List<string> m_messageList;

    /// <summary>
    /// 現在表示している文字のインデックス
    /// </summary>
#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    int m_index = 0;

    /// <summary>
    /// InputSystemの編集用マップ
    /// </summary>
    InputActionMapSettings m_inputMap;

    /// <summary>
    /// フェードで使用するコルーチンのアクティブなものを保持するフィールド
    /// </summary>
    Coroutine m_activeCoroutine;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        m_inputMap = new();
        m_inputMap.UI.Any.started += OnAnyInput;
        m_inputMap.Enable();

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

        m_activeCoroutine = StartCoroutine(CoAwake());
    }

    /// <summary>
    /// InputSystemのイベントハンドラ
    /// </summary>
    private void OnAnyInput(InputAction.CallbackContext context)
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
            if (m_activeCoroutine != null)
            {
                StopCoroutine(m_activeCoroutine);
                m_text.text = m_messageList[m_index];
            }
            m_index++;
            m_activeCoroutine = StartCoroutine(CoFade());
        }
        else
        {
            m_inputMap.UI.Any.started -= OnAnyInput;
            m_inputMap.Disable();

            StopCoroutine(m_activeCoroutine);
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
        m_activeCoroutine = null;
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
        m_activeCoroutine = null;
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
        Initiate.Fade(Name.Scene.Title, Color.black, 1.0f);
    }
}