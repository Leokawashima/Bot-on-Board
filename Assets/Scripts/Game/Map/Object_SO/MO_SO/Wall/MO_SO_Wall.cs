using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/Wall")]
public class MO_SO_Wall : MapObject_SO_Template, IDestroy
{
    [field: SerializeField] public uint TurnMax { get; set; }
    [field: SerializeField] public uint TurnSpawn { get; set; }
}