using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrailSystem : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera m_Camera;
    [SerializeField] CinemachineSmoothPath m_SmoothMath;

    CinemachineTrackedDolly m_Dolly;

    private void OnEnable()
    {
        PlayerInputManager.OnMouseSubClickEvent += OnCMouse_SubClick;
    }

    private void OnDisable()
    {
        PlayerInputManager.OnMouseSubClickEvent -= OnCMouse_SubClick;
    }

    private void Start()
    {
        m_Dolly = m_Camera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    void OnCMouse_SubClick()
    {
        m_Dolly.m_PathPosition = Time.time % 4;
    }
}
