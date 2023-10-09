using System;
using UnityEngine;
using UnityEngine.UI;

public class EscMenuManager : MonoBehaviour
{
    [SerializeField] Canvas m_Canvas;
    [SerializeField] AudioSource m_Audio;
    [SerializeField] Button m_CloseButton;
    [SerializeField] SoundVolumeManager m_SoundVolumeManager;

    public static Action Event_EscMenuOpen;
    public static Action Event_EscMenuClose;

    bool m_IsOpen = false;

    void Start()
    {
        m_CloseButton.onClick.AddListener(Close);

        m_SoundVolumeManager.Initialize();
        m_SoundVolumeManager.Load();
    }

    public void Switch()
    {
        if(m_IsOpen == false)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    void Open()
    {
        m_Audio.Play();
        m_IsOpen = true;
        m_Canvas.gameObject.SetActive(true);

        Event_EscMenuOpen?.Invoke();
    }

    void Close()
    {
        m_Audio.Play();
        m_IsOpen = false;
        if (m_SoundVolumeManager.IsDirty) m_SoundVolumeManager.Save();
        m_Canvas.gameObject.SetActive(false);

        Event_EscMenuClose?.Invoke();
    }
}