using System;
using UnityEngine;
using static Game.GameManager;

namespace Game
{
    [Serializable]
    public class GameDebug
    {
        [SerializeField] private GameState m_gameState = GameState.Non;
        [SerializeField] private BattleState m_battleState = BattleState.Non;

        GameDebug()
        {
            Event_Initialize += SetGameState(GameState.Initialize);
            Event_Initialize += SetBattleState(BattleState.Non);

            Event_Turn_Initialize += SetGameState(GameState.Battle);
            Event_Turn_Initialize += SetBattleState(BattleState.Initialize);

            Event_Turn_Place += SetBattleState(BattleState.Place);

            Event_Turn_TurnEnd += SetBattleState(BattleState.TurnEnd);

            Event_Turn_AIAction += SetBattleState(BattleState.AIAction);

            Event_Turn_Finalize += SetBattleState(BattleState.Finalize);

            Event_Turn_GameSet += SetBattleState(BattleState.GameSet);

            Event_Finalize += SetGameState(GameState.Finalize);
            Event_Finalize += SetBattleState(BattleState.Non);
        }
        ~GameDebug()
        {
            Event_Initialize -= SetGameState(GameState.Initialize);
            Event_Initialize -= SetBattleState(BattleState.Non);

            Event_Turn_Initialize -= SetGameState(GameState.Battle);
            Event_Turn_Initialize -= SetBattleState(BattleState.Initialize);

            Event_Turn_Place -= SetBattleState(BattleState.Place);

            Event_Turn_TurnEnd -= SetBattleState(BattleState.TurnEnd);

            Event_Turn_AIAction -= SetBattleState(BattleState.AIAction);

            Event_Turn_Finalize -= SetBattleState(BattleState.Finalize);

            Event_Turn_GameSet -= SetBattleState(BattleState.GameSet);

            Event_Finalize -= SetGameState(GameState.Finalize);
            Event_Finalize -= SetBattleState(BattleState.Non);
        }

        private Action SetGameState(GameState state_)
        {
            return () => m_gameState = state_;
        }
        private Action SetBattleState(BattleState state_)
        {
            return () => m_battleState = state_;
        }
    }
}