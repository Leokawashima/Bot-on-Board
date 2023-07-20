using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyGameSystem : MonoBehaviour
{
    enum BattleState { Non, SetFirst, SetSecond, AIAction }
    [SerializeField] BattleState battleState = BattleState.Non;

    Action GameUpdate;
    [SerializeField] int Cnt = 0;

    void Start()
    {
        GameUpdate = GameInitialize;
    }

    void Update()
    {
        GameUpdate?.Invoke();
    }

    void GameInitialize()
    {
        Debug.Log("Initialize");
        battleState = BattleState.SetFirst;
        GameUpdate = GameLoop;
    }
    void GameLoop()
    {
        Debug.Log("Loop");
        switch (battleState)
        {
            case BattleState.SetFirst:
                {
                    Debug.Log("First");
                    battleState = BattleState.SetSecond;
                }
                break;
            case BattleState.SetSecond:
                {
                    Debug.Log("Second");
                    battleState = BattleState.AIAction;
                }
                break;
            case BattleState.AIAction:
                {
                    Debug.Log("AI");
                    if (Cnt++ >= 3)
                    {
                        GameUpdate = GameFinalize;
                    }
                    else
                    {
                        battleState = BattleState.SetFirst;
                    }
                }
                break;
        }
    }
    void GameFinalize()
    {
        Debug.Log("Finalize");
        GameUpdate = null;
    }
}
