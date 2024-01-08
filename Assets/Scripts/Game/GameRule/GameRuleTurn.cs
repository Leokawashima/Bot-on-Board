using UnityEngine;
using Bot;
using Player;
using Map;

namespace Game.GameRule
{
    public class GameRuleTurn : GameRule_Template
    {
        [field: SerializeField] public int TurnSuddenDeath { get; private set; } = 30;
        [field: SerializeField] public int TurnForceFinish { get; private set; } = 50;

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
            MapManager.Singleton.MapCreate();
        }

        private void OnMapCreated()
        {
            GUIManager.Singleton.CutIn.Play($"Turn:{TurnElapsed}", () =>
            {
                CallEventInitialize();
                BotManager.Singleton.Initialize();
                GUIManager.Singleton.Initialize();
                GUIManager.Singleton.PlayerUI.TurnInitialize();
                
                GUIManager.Singleton.CutIn.Play($"Player{ProgressIndex}\n○○のターン！", () =>
                {
                    GUIManager.Singleton.PlayerUI.Enable(ProgressIndex);
                });
            });
        }

        private void OnBotActioned()
        {
            if (IsGameSet())
            {
                GUIManager.Singleton.CutIn.Play("GameSet", () =>
                {
                    CallEventGameSet();
                    GameManager.Singleton.SystemFinalize();
                });
            }
            else
            {
                CallEventFinalize();

                NextTurn();
                ResetProgress();

                GUIManager.Singleton.CutIn.Play($"Turn:{TurnElapsed}", () =>
                {
                    CallEventInitialize();
                    GUIManager.Singleton.PlayerUI.TurnInitialize();
                    GUIManager.Singleton.CutIn.Play($"Player{ProgressIndex}\n○○のターン！", () =>
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
                GUIManager.Singleton.CutIn.Play($"Place:{ProgressIndex}", () =>
                {
                    GUIManager.Singleton.PlayerUI.Enable(ProgressIndex);
                });
            }
            else
            {
                GUIManager.Singleton.CutIn.Play("AIの行動！", () =>
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