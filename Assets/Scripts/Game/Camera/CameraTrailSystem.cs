using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrailSystem : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera m_Camera;
    [SerializeField] CinemachineSmoothPath m_SmoothMath;
    [SerializeField] float m_DragSpeed = 0.005f;

    CinemachineTrackedDolly m_Dolly;
    int m_PathWayPoints = 0;

    private void Start()
    {
        m_Dolly = m_Camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        m_PathWayPoints = m_SmoothMath.m_Waypoints.Length;
    }

    private void Update()
    {
        var _value = Time.deltaTime * m_DragSpeed;
        m_Dolly.m_PathPosition = (m_Dolly.m_PathPosition + _value) % m_PathWayPoints;
    }
}
