using UnityEngine;

public class BotSetting : MonoBehaviour
{
    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public uint HP { get; private set; }
    [field: SerializeField] public uint HPMax { get; private set; }
    [field: SerializeField] public uint Attack { get; private set; }
}