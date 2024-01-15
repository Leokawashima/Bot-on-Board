using System;
using UnityEngine;

namespace Game.GameRule
{
    public abstract class GameRule_Template : MonoBehaviour
    {
        [field: SerializeField] public DateTime TimeCreated { get; private set; }
        [field: SerializeField] public DateTime TimeElapsed { get; private set; }

        [field: SerializeField] public int TurnElapsed { get; private set; } = 0;

        [field: SerializeField] public int ProgressIndex { get; protected set; } = 0;

        public static event Action
            Event_Initialize,
            Event_Place,
            Event_TurnEnd,
            Event_AIAction,
            Event_Finalize,
            Event_GameSet;

        public static event Action<int>
            Event_TurnChanged,
            Event_ProgressChanged;

        protected void CallEventInitialize()
        {
            Event_Initialize?.Invoke();
        }
        protected void CallEventPlace()
        {
            Event_Place?.Invoke();
        }
        protected void CallEventTurnEnd()
        {
            Event_TurnEnd?.Invoke();
        }
        protected void CallEventAIAction()
        {
            Event_AIAction?.Invoke();
        }
        protected void CallEventFinalize()
        {
            Event_Finalize?.Invoke();
        }
        protected void CallEventGameSet()
        {
            Event_GameSet?.Invoke();
        }

        protected void NextTurn()
        {
            TurnElapsed++;
            Event_TurnChanged?.Invoke(TurnElapsed + 1);
        }
        protected void ResetTurn()
        {
            TurnElapsed = 0;
            Event_TurnChanged?.Invoke(TurnElapsed + 1);
        }
        protected void NextProgress()
        {
            ProgressIndex++;
            Event_ProgressChanged?.Invoke(ProgressIndex);
        }
        protected void ResetProgress()
        {
            ProgressIndex = 0;
            Event_ProgressChanged?.Invoke(ProgressIndex);
        }

        public abstract void Initialize();
        public abstract bool IsGameSet();
    }
}