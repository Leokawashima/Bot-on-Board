using System;
using UnityEngine;
using Bot;
using Player;
using Map;
using GameMode;

namespace Game.GameRule
{
    public class GameRuleTutorial : GameRule_Template
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
            TutorialManager.Event_SingletonCreated += OnSingletonCreated;

            static void OnSingletonCreated()
            {
                TutorialManager.Initialize();
                TutorialManager.Enable(0, MapManager.Singleton.MapCreate);
                TutorialManager.Event_SingletonCreated -= OnSingletonCreated;
            }
        }

        private void OnMapCreated()
        {
            TutorialManager.Enable(1,() =>
                TurnCutIn(() =>
                {
                    CallEventInitialize();
                    BotManager.Singleton.Initialize();
                    CameraManager.Singleton.Initialize();
                    GUIManager.Singleton.Initialize();

                    PlayerCutIn(() =>
                    {
                        TutorialManager.Enable(2, () =>
                        {
                            GUIManager.Singleton.PlayerUI.Enable(ProgressIndex);
                        });
                    });
                })
                );
        }

        private void OnBotActioned()
        {
            TutorialManager.Enable(4, () =>
            {
                GameSetCutIn(() =>
                {
                    CallEventGameSet();
                    GameManager.Singleton.SystemFinalize();
                });
            });
        }
        private void OnButton_TurnEnd()
        {
            TutorialManager.Enable(3, () =>
            {
                AICutIn(() =>
                {
                    CallEventAIAction();
                    BotManager.Singleton.Action();
                });
            });
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