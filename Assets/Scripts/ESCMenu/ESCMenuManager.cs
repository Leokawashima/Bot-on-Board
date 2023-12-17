using System;
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

    public static Action
        Event_Open,
        Event_Close;

    void Start()
    {
        m_canvas.enabled = false;
        m_closeButton.onClick.AddListener(Close);

        // サウンド初期化とファイル読み込み
        m_soundVolumeManager.Initialize();
        m_soundVolumeManager.Load();
    }

    public void Switch()
    {
        // わかりやすさ重視
        if(false == GlobalSystem.IsPause)
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
        GlobalSystem.SetPause(true);
        m_audio.Play();
        m_canvas.enabled = true;

        Event_Open?.Invoke();
    }

    void Close()
    {
        m_audio.Play();
        if (m_soundVolumeManager.IsDirty)
        {
            m_soundVolumeManager.Save();
        }
        m_canvas.enabled = false;

        Event_Close?.Invoke();
        GlobalSystem.SetPause(false);
    }
}