using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Singleton;

    [SerializeField] CinemachineInputProvider m_InputProvider;

    void Awake()
    {
        Singleton = this;
    }

    private void Update()
    {
        SetFreeLookCamIsMove(false == EventSystem.current.IsPointerOverGameObject());
    }

    public void SetFreeLookCamIsMove(bool flag_)
    {
        m_InputProvider.enabled = flag_;
    }
}