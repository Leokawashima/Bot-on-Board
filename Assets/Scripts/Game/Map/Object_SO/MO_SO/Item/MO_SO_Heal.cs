using UnityEngine;

[CreateAssetMenu(menuName = "MO_SO/Heal")]
public class MO_SO_Heal : MapObject_SO_Template, IDestroy, IHeal
{
    [field: SerializeField] public uint TurnMax { get; set; }
    [field: SerializeField] public uint TurnSpawn { get; set; }

    [field: SerializeField] public float HealPower { get; set; }

    public bool CheckHealCollider()
    {
        return true;
    }

    public void Heal()
    {

    }
}
