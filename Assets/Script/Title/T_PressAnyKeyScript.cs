using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_PressAnyKeyScript : MonoBehaviour
{
    [SerializeField] FlashSystem m_Flash;
    public FlashSystem Flash { get { return m_Flash; } }
    [SerializeField] RainbowText m_Rainbow;
    [SerializeField] AudioSource m_Audio;
    [SerializeField] ParticleSystem m_Particle;

    private void Start()
    {
        m_Rainbow.Rainbow();
    }

    public void Enable()
    {
        m_Rainbow.gameObject.SetActive(true);
        m_Particle.gameObject.SetActive(true);
    }

    public void Disable()
    {
        m_Rainbow.gameObject.SetActive(false);
        m_Particle.gameObject.SetActive(false);
    }

    public void EnterEvent()
    {
        m_Flash.Flash();
        m_Audio.Play();
        m_Particle.Play();
    }
}
