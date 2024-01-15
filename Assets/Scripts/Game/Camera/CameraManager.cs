using UnityEngine;
using Cinemachine;
using Bot;
using UnityEngine.UI;
using System.Collections;

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

    [SerializeField] CinemachineVirtualCamera m_cam;
    [SerializeField] CinemachineSmoothPath m_smoothPath;
    public void Animation()
    {
        StartCoroutine(CoAnim());
    }
    private IEnumerator CoAnim()
    {
        m_cam.Priority = 30;
        var _dolly = m_cam.GetCinemachineComponent<CinemachineTrackedDolly>();
        var _points = m_smoothPath.m_Waypoints.Length;

        var m_elapsed = 0.0f;
        while (m_elapsed < 4.0f)
        {
            m_elapsed += Time.deltaTime;
            _dolly.m_PathPosition = m_elapsed + 1.0f;
            yield return null;
        }
        m_cam.Priority = 10;
    }
}