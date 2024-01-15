using UnityEngine;
using Cinemachine;
using Bot;
using UnityEngine.UI;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    [SerializeField] private CinemachineInputProvider m_InputProvider;
    [SerializeField] private CameraChangeSystem m_changeSystem;
    [SerializeField] private Button m_changeButton;

    public void Initialize()
    {
        m_changeButton.onClick.AddListener(m_changeSystem.Next);
        for (int i = 0, cnt = BotManager.Singleton.Bots.Count; i < cnt; ++i)
        {
            m_changeSystem.AddCamera(BotManager.Singleton.Bots[i].Camera.Default);
        }
    }
    public void SetFreeLookCamIsMove(bool flag_)
    {
        m_InputProvider.enabled = flag_;
    }
}