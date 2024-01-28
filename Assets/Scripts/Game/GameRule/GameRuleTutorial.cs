using System.Collections;
using UnityEngine;
using Bot;
using Player;
using Map;

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
            TutorialManager.Event_SingletonCreated += OnSingletonCreated;

            void OnSingletonCreated()
            {
                TutorialManager.Initialize();
                TutorialManager.Enable(0, MapManager.Singleton.MapCreate);
                TutorialManager.Event_SingletonCreated -= OnSingletonCreated;
            }
        }

        private void OnMapCreated()
        {
            GUIManager.Singleton.CutInTurn.Play("ターン", () =>
            {
                CallEventInitialize();
                BotManager.Singleton.Initialize();
                CameraManager.Singleton.Initialize();
                GUIManager.Singleton.Initialize();
                
                GUIManager.Singleton.CutInDefault.Play($"Player{ProgressIndex + 1}\n○○のターン！", () =>
                {
                    GUIManager.Singleton.PlayerUI.Enable(ProgressIndex);
                });
            });
        }

        private void OnBotActioned()
        {
            if (IsGameSet())
            {
                GUIManager.Singleton.CutInDefault.Play("ゲームセット", () =>
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

                GUIManager.Singleton.CutInTurn.Play("ターン", () =>
                {
                    CallEventInitialize();
                    GUIManager.Singleton.PlayerUI.TurnInitialize();
                    GUIManager.Singleton.CutInDefault.Play($"Player{ProgressIndex + 1}\n○○のターン！", () =>
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
                GUIManager.Singleton.CutInDefault.Play($"Player{ProgressIndex + 1}\n○○のターン！", () =>
                {
                    GUIManager.Singleton.PlayerUI.Enable(ProgressIndex);
                });
            }
            else
            {
                GUIManager.Singleton.CutInDefault.Play("AIの行動！", () =>
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
    }
}