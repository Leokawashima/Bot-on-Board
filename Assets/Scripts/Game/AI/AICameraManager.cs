using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace AI
{
    public class AICameraManager : MonoBehaviour
    {
        [SerializeField] CameraChangeSystem cameraSystem;

        private InputActionMapSettings m_input;

        private void Awake()
        {
            m_input = new();
            m_input.Player.Debug.started += OnCameraChenge;
            m_input.Enable();
        }
        private void OnDestroy()
        {
            m_input.Player.Debug.started -= OnCameraChenge;
            m_input.Disable();
            m_input = null;
        }

        public void Initialize()
        {
            cameraSystem.m_Cameras = new(AIManager.Singleton.AIList.Count + 1);
            for (int i = 0, cnt = AIManager.Singleton.AIList.Count; i < cnt; ++i)
            {
                cameraSystem.m_Cameras.Add(AIManager.Singleton.AIList[i].Camera.Default);
            }
            cameraSystem.Refresh();
        }

        private void OnCameraChenge(InputAction.CallbackContext context_)
        {
            cameraSystem.Change();
        }
    }
}