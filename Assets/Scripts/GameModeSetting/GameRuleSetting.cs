using UnityEngine;

public class GameRuleSetting : MonoBehaviour
{
    [field: SerializeField] public int TurnSuddenDeath { get; private set; } = 30;
    [field: SerializeField] public int TurnForceFinish { get; private set; } = 50;

    public void Initiaiize()
    {

    }
}