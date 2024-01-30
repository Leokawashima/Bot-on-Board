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
        [field: SerializeField] public AnimatorSystem Animator { get; private set; }

        [SerializeField] Renderer m_ear1;
        [SerializeField] Renderer m_ear2;

        private readonly float[] m_COLORS = new float[]
    {
        0.0f,
        240.0f / 360.0f,
        120.0f / 360.0f,
        60.0f / 360.0f,
    };

        public void Initialize(PlayerAgent operator_, BotSetting setting_, Vector2Int pos_)
        {
            name = $"Bot_{operator_.Index}";
            Operator = operator_;

            Health = new(this, setting_);
            Assault = new(this, setting_);
            Brain = new(this);
            Travel = new(this, pos_);
            Perform = new(this);
            m_ear1.material.color = Color.HSVToRGB(m_COLORS[operator_.Index], 1.0f, 1.0f);
            m_ear2.material.color = Color.HSVToRGB(m_COLORS[operator_.Index], 1.0f, 1.0f);
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