using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class M_MessageScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_Text;
    [SerializeField] uint m_Index = 0;
    [SerializeField] bool m_IsFade = false;
    [SerializeField] float m_FadeTime = 1.0f;
    [SerializeField] string[] m_Message;
    
    InputActionMapSettings m_InputMap;

    void Start()
    {
        m_InputMap = new();
        m_InputMap.UI.Any.started += OnAnyInput;
        m_InputMap.Enable();

        for (int i = 0; i < m_Message.Length; ++i)
        {
            if(m_Message[i].Contains("\\n"))
                m_Message[i] = m_Message[i].Replace("\\n", Environment.NewLine);
        }
        m_Text.text = m_Message[m_Index];
        StartCoroutine(CoStart());
    }

    void OnAnyInput(InputAction.CallbackContext context)
    {
        SetNextMessage();
    }
    
    void SetNextMessage()
    {
        if(m_Index + 1 != m_Message.Length)
        {
            if (m_IsFade)
            {
                m_IsFade = false;
                StopAllCoroutines();
                m_Text.text = m_Message[m_Index];
            }
            m_Index++;
            StartCoroutine(CoFade());
        }
        else
        {
            m_InputMap.UI.Any.started -= OnAnyInput;
            m_InputMap.Disable();

            StopAllCoroutines();
            StartCoroutine(CoSceneLoad());
        }
    }

    IEnumerator CoStart()
    {
        m_IsFade = true;
        var reset = m_Text.color;
        reset.a = 0;
        m_Text.color = reset;

        while(true)
        {
            var color = m_Text.color;
            color.a += Time.deltaTime / m_FadeTime;
            m_Text.color = color;
            if(m_Text.color.a >= 1)
            {
                break;
            }
            yield return null;
        }

        m_IsFade = false;
    }

    IEnumerator CoSceneLoad()
    {
        m_IsFade = true;

        while(true)
        {
            var color = m_Text.color;
            color.a -= Time.deltaTime / m_FadeTime;
            m_Text.color = color;
            if(m_Text.color.a <= 0)
            {
                Initiate.Fade(Name.Scene.Title, Color.black, 1.0f);
                break;
            }
            yield return null;
        }
    }

    IEnumerator CoFade()
    {
        m_IsFade = true;

        while (true)
        {
            var color = m_Text.color;
            color.a -= Time.deltaTime / m_FadeTime;
            m_Text.color = color;
            if(m_Text.color.a <= 0)
                break;
            yield return null;
        }

        m_Text.text = m_Message[m_Index];
        yield return null;

        while(true)
        {
            var color = m_Text.color;
            color.a += Time.deltaTime / m_FadeTime;
            m_Text.color = color;
            if(m_Text.color.a >= 1)
                break;
            yield return null;
        }

        m_IsFade = false;
    }

}