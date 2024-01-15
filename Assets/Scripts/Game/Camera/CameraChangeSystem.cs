using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChangeSystem : MonoBehaviour
{
    [SerializeField] private List<CinemachineVirtualCameraBase> m_cameras = new();

    [SerializeField] private int m_index = 0;

    public void AddCamera(CinemachineVirtualCameraBase cam_)
    {
        m_cameras.Add(cam_);

        for (int i = 0, cnt = m_cameras.Count - 1; i < cnt; ++i)
        {
            if (m_cameras[i].Priority < m_cameras[i + 1].Priority)
            {
                m_index = i + 1;
            }
        }
    }

    public void Change(int to_)
    {
        if (to_ >= m_cameras.Count)
        {
            return;
        }

        (m_cameras[to_].Priority, m_cameras[m_index].Priority) = (m_cameras[m_index].Priority, m_cameras[to_].Priority);
        m_index = to_;
    }
    public void Next()
    {
        var _next = (m_index + 1) % m_cameras.Count;

        (m_cameras[_next].Priority, m_cameras[m_index].Priority) = (m_cameras[m_index].Priority, m_cameras[_next].Priority);
        m_index = _next;
    }
}
