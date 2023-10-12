using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Singleton;

    [SerializeField] CinemachineInputProvider m_InputProvider;

    void Awake()
    {
        Singleton = this;
    }

    public void SetFreeLookCamIsMove(bool flag_)
    {
        m_InputProvider.enabled = flag_;
    }
}