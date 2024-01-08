using System;
using UnityEngine;
using Game.GameRule;

namespace Game
{
    [Serializable]
    public class GameDebug
    {
        [SerializeField] private GameState m_gameState = GameState.Non;
        [SerializeField] private BattleState m_battleState = BattleState.Non;

        GameDebug()
        {
            GameManager.Event_Initialize += SetGameState(GameState.Initialize);
            GameManager.Event_Initialize += SetBattleState(BattleState.Non);

            GameRule_Template.Event_Initialize += SetGameState(GameState.Battle);
            GameRule_Template.Event_Initialize += SetBattleState(BattleState.Initialize);

            GameRule_Template.Event_Place += SetBattleState(BattleState.Place);

            GameRule_Template.Event_TurnEnd += SetBattleState(BattleState.TurnEnd);

            GameRule_Template.Event_AIAction += SetBattleState(BattleState.AIAction);

            GameRule_Template.Event_Finalize += SetBattleState(BattleState.Finalize);

            GameRule_Template.Event_GameSet += SetBattleState(BattleState.GameSet);

            GameManager.Event_Finalize += SetGameState(GameState.Finalize);
            GameManager.Event_Finalize += SetBattleState(BattleState.Non);
        }
        ~GameDebug()
        {
            GameManager.Event_Initialize -= SetGameState(GameState.Initialize);
            GameManager.Event_Initialize -= SetBattleState(BattleState.Non);

            GameRule_Template.Event_Initialize -= SetGameState(GameState.Battle);
            GameRule_Template.Event_Initialize -= SetBattleState(BattleState.Initialize);

            GameRule_Template.Event_Place -= SetBattleState(BattleState.Place);

            GameRule_Template.Event_TurnEnd -= SetBattleState(BattleState.TurnEnd);

            GameRule_Template.Event_AIAction -= SetBattleState(BattleState.AIAction);

            GameRule_Template.Event_Finalize -= SetBattleState(BattleState.Finalize);

            GameRule_Template.Event_GameSet -= SetBattleState(BattleState.GameSet);

            GameManager.Event_Finalize -= SetGameState(GameState.Finalize);
            GameManager.Event_Finalize -= SetBattleState(BattleState.Non);
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