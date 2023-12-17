using UnityEngine;
using Cinemachine;

namespace AI
{
    public class AICamera : MonoBehaviour
    {
        [field: SerializeField]
        public CinemachineVirtualCameraBase Default { get; private set; }
    }
}