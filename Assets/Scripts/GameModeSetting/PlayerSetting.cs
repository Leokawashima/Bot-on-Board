using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    [field: SerializeField] public uint BotOperations { get; private set; }
    [field: SerializeField] public BotSetting[] BotSettings { get; private set; }
}