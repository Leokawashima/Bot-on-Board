﻿using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// EscMenuを管理するクラス
/// </summary>
public class EscMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private AudioSource m_audio;
    [SerializeField] private Button m_closeButton;
    [SerializeField] private SoundVolumeManager m_soundVolumeManager;

    public static Action Event_EscMenuOpen;
    public static Action Event_EscMenuClose;

    bool m_isOpen = false;

    void Start()
    {
        m_closeButton.onClick.AddListener(Close);

        // サウンド初期化とファイル読み込み
        m_soundVolumeManager.Initialize();
        m_soundVolumeManager.Load();
    }

    public void Switch()
    {
        // わかりやすさ重視
        if(false == m_isOpen)
            Open();
        else
            Close();
    }

    void Open()
    {
        m_audio.Play();
        m_isOpen = true;
        m_canvas.gameObject.SetActive(true);

        Event_EscMenuOpen?.Invoke();
    }

    void Close()
    {
        m_audio.Play();
        m_isOpen = false;
        if (m_soundVolumeManager.IsDirty) m_soundVolumeManager.Save();
        m_canvas.gameObject.SetActive(false);

        Event_EscMenuClose?.Invoke();
    }
}