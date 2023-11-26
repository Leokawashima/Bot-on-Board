using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChangeSystem : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] m_Cameras;
    [SerializeField] int m_Index = 0;

    private void Start()
    {
        for (int i = 0; i < m_Cameras.Length - 1; ++i)
        {
            if (m_Cameras[i].Priority < m_Cameras[i + 1].Priority)
                m_Index = i + 1;
        }
    }

    void Change()
    {
        m_Cameras[m_Index].Priority = 0;
        m_Index = ++m_Index % m_Cameras.Length;
        m_Cameras[m_Index].Priority = 10;
    }
}
