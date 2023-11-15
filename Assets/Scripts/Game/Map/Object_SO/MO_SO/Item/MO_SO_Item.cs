using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/Default")]
public class MO_SO_Item : MapObject_SO_Template, IDestroy
{
    [field: SerializeField] public uint TurnMax { get; set; }
    [field: SerializeField] public uint TurnSpawn { get; set; }
}