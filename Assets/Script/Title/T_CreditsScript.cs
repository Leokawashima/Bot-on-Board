using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_CreditsScript : MonoBehaviour
{
    [SerializeField] GameObject m_Credits;
    [SerializeField] Button m_ButtonBack;
    [SerializeField] AudioSource m_Audio;

    public event Action OnHideCredits;

    void Start()
    {
        m_ButtonBack.onClick.AddListener(OnClickBack);
    }

    void OnClickBack()
    {
        m_Audio.Play();
        OnHideCredits?.Invoke();
    }

    public void Enable()
    {
        m_Credits.SetActive(true);
    }
    public void Disable()
    {
        m_Credits.SetActive(false);
    }
}
