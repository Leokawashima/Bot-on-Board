using UnityEngine;

public class ESCMenuManager : MonoBehaviour
{
    [SerializeField] SoundVolumeManager m_SoundVolumeManager;

    void Start()
    {
        m_SoundVolumeManager.Initialize();
        m_SoundVolumeManager.Load();
    }

    public void OnSwitch()
    {

    }

    void Open()
    {

    }

    void Close()
    {
        if (m_SoundVolumeManager.IsDirty) m_SoundVolumeManager.Save();
    }
}