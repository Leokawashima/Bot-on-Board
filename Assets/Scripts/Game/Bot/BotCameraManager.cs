using UnityEngine;
using UnityEngine.InputSystem;

namespace Bot
{
    public class BotCameraManager : MonoBehaviour
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
            for (int i = 0, cnt = BotManager.Singleton.Bots.Count; i < cnt; ++i)
            {
                cameraSystem.m_Cameras.Add(BotManager.Singleton.Bots[i].Camera.Default);
            }
            cameraSystem.Refresh();
        }

        private void OnCameraChenge(InputAction.CallbackContext context_)
        {
            cameraSystem.Change();
        }
    }
}