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
    Vector2 m_PrePos = new();
    int m_PathWayPoints = 0;

    private void Start()
    {
        m_Dolly = m_Camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        m_PathWayPoints = m_SmoothMath.m_Waypoints.Length;
    }

    void OnDragStart()
    {
        m_PrePos = PlayerInputManager.m_Pos;
    }
    void OnDrag()
    {
        var _value = (PlayerInputManager.m_Pos.x - m_PrePos.x) * m_DragSpeed;
        m_Dolly.m_PathPosition = (m_Dolly.m_PathPosition + _value) % m_PathWayPoints;

        m_PrePos = PlayerInputManager.m_Pos;
    }
}
