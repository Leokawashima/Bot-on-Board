using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_MenuScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button m_Start;
    [SerializeField] Button m_Quit;
    [SerializeField] Button m_Credits;
    [Header("Sound")]
    [SerializeField] AudioSource m_Audio;
    [SerializeField] Animator m_Animator;

    public event Action OnMenuStart;
    public event Action OnMenuCredits;

    void Start()
    {
        m_Start.onClick.AddListener(OnStart);
        m_Quit.onClick.AddListener(OnQuit);
        m_Credits.onClick.AddListener(OnCredits);
    }

    public void Enable()
    {
        m_Start.gameObject.SetActive(true);
        m_Quit.gameObject.SetActive(true);
        m_Credits.gameObject.SetActive(true);
    }

    public void Disable()
    {
        m_Start.gameObject.SetActive(false);
        m_Quit.gameObject.SetActive(false);
        m_Credits.gameObject.SetActive(false);
    }

    void OnStart()
    {
        m_Audio.Play();
        OnMenuStart?.Invoke();
    }

    void OnQuit()
    {
        m_Audio.Play();
        m_Animator.SetTrigger("Off");

#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false;
#else
        //Application.Quit();
#endif
    }

    void OnCredits()
    {
        m_Audio.Play();
        OnMenuCredits?.Invoke();
    }
}
