using UnityEngine;
using Player;

/// <summary>
/// Bot単体を処理するクラス
/// </summary>
namespace Bot
{
    public class BotAgent : MonoBehaviour
    {
        /// <summary>
        /// このAIを保持しているPlayer
        /// </summary>
        public PlayerAgent Operator { get; private set; }

        [field: SerializeField] public BotHealth Health { get; private set; }
        [field: SerializeField] public BotAssault Assault { get; private set; }
        [field: SerializeField] public BotBrain Brain { get; private set; }
        [field: SerializeField] public BotTravel Travel { get; private set; }
        [field: SerializeField] public BotPerform Perform { get; private set; }

        [field: SerializeField] public BotCamera Camera { get; private set; }

        public BotAgent Spawn(PlayerAgent operator_, Vector2Int pos_)
        {
            name = $"Bot：{operator_.Index}";
            Operator = operator_;

            Health = new(this);
            Assault = new(this);
            Brain = new(this);
            Travel = new(this, pos_);
            Perform = new(this);

            return this;
        }

        public void Think()
        {
            Travel.Clear();
            Brain.Think(this);
        }

        public void Action()
        {
            Perform.Action();
            Perform.Clear();
        }
    }
}