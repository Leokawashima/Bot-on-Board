using UnityEngine;
using Bot;
using Player;
using Map;
using System;
using GameMode;

namespace Game.GameRule
{
    public class GameRuleTurn : GameRule_Template
    {
        [field: SerializeField] public int TurnSuddenDeath { get; private set; }
        [field: SerializeField] public int TurnForceFinish { get; private set; }

        private void OnEnable()
        {
            MapManager.Event_MapCreated += OnMapCreated;
            BotManager.Event_BotsActioned += OnBotActioned;
            PlayerUIManager.Event_ButtonTurnEnd += OnButton_TurnEnd;
        }
        private void OnDisable()
        {
            MapManager.Event_MapCreated -= OnMapCreated;
            BotManager.Event_BotsActioned -= OnBotActioned;
            PlayerUIManager.Event_ButtonTurnEnd -= OnButton_TurnEnd;
        }

        public override void Initialize()
        {
            //CameraManager.Singleton.Animation();
            TurnSuddenDeath = GameModeManager.TurnSuddonDeath;
            TurnForceFinish = GameModeManager.TurnForceFinish;
            ResetTurn();
            MapManager.Singleton.MapCreate();
        }

        private void OnMapCreated()
        {
            TurnCutIn(() =>
            {
                CallEventInitialize();
                BotManager.Singleton.Initialize();
                CameraManager.Singleton.Initialize();
                GUIManager.Singleton.Initialize();

                PlayerCutIn(() =>
                {
                    GUIManager.Singleton.PlayerUI.Enable(ProgressIndex);
                });
            });
        }

        private void OnBotActioned()
        {
            if (IsGameSet())
            {
                GameSetCutIn(() =>
                {
                    CallEventGameSet();
                    GameManager.Singleton.SystemFinalize();
                });
            }
            else
            {
                CallEventFinalize();

                MapManager.Singleton.TurnUpdate();

                NextTurn();
                ResetProgress();

                TurnCutIn(() =>
                {
                    CallEventInitialize();
                    GUIManager.Singleton.PlayerUI.TurnInitialize();
                    PlayerCutIn(() =>
                    {
                        GUIManager.Singleton.PlayerUI.Enable(ProgressIndex);
                    });
                });
            }
        }
        private void OnButton_TurnEnd()
        {
            if (++ProgressIndex < PlayerManager.Singleton.Players.Count)
            {
                CallEventTurnEnd();

                CallEventPlace();
                PlayerCutIn(() =>
                {
                    GUIManager.Singleton.PlayerUI.Enable(ProgressIndex);
                });
            }
            else
            {
                AICutIn(() =>
                {
                    CallEventAIAction();
                    BotManager.Singleton.Action();
                });
            }
        }


        public override bool IsGameSet()
        {
            return BotManager.Singleton.CheckBotDead() || TurnElapsed > TurnForceFinish;
        }

        private void TurnCutIn(Action callback_)
        {
            GUIManager.Singleton.CutInTurn.Play("ターン", callback_);
        }
        private void PlayerCutIn(Action callback_)
        {
            GUIManager.Singleton.CutInDefault.Play($"Player{ProgressIndex + 1}\n{PlayerManager.Singleton.Players[ProgressIndex].Name}\nのターン！", callback_);
        }
        private void AICutIn(Action callback_)
        {
            GUIManager.Singleton.CutInDefault.Play("AIの行動！", callback_);
        }
        private void GameSetCutIn(Action callback_)
        {
            GUIManager.Singleton.CutInDefault.Play("ゲームセット", callback_);
        }
    }
}