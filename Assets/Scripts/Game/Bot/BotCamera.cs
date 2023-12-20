using UnityEngine;
using Cinemachine;

namespace Bot
{
    public class BotCamera : MonoBehaviour
    {
        [field: SerializeField]
        public CinemachineVirtualCameraBase Default { get; private set; }
    }
}