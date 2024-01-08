using UnityEngine;

namespace Bot
{
    public class BotCameraManager : MonoBehaviour
    {
        [SerializeField] CameraChangeSystem cameraSystem;

        public void Initialize()
        {
            for (int i = 0, cnt = BotManager.Singleton.Bots.Count; i < cnt; ++i)
            {
                cameraSystem.m_Cameras.Add(BotManager.Singleton.Bots[i].Camera.Default);
            }
            cameraSystem.Refresh();
        }

        private void OnCameraChenge()
        {
            cameraSystem.Change();
        }
    }
}