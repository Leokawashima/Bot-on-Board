using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AICameraTest : MonoBehaviour
{
    [SerializeField] CameraChangeSystem cameraSystem;
    [SerializeField] CinemachineVirtualCameraBase mapCam;
    private InputActionMapSettings m_inputMap;
    [SerializeField] AIManager AIManager;
    [SerializeField] CinemachineBlenderSettings blenderSettings;

    private void OnEnable()
    {
        m_inputMap = new();
        m_inputMap.Player.Debug.started += Input;
        m_inputMap.Enable();
    }
    private void OnDisable()
    {
        m_inputMap.Player.Debug.started -= Input;
        m_inputMap.Disable();
    }

    public void Initialize()
    {
        cameraSystem.m_Cameras = new(AIManager.AIList.Count + 1)
        {
            mapCam
        };
        for (int i = 0; i < AIManager.AIList.Count; ++i)
        {
            cameraSystem.m_Cameras.Add(AIManager.AIList[i].cameraA);
        }
        cameraSystem.Refresh();
    }

    private void Input(InputAction.CallbackContext context_)
    {
        cameraSystem.Change();
    }
}