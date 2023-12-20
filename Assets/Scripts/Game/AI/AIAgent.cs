using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Player;

/// <summary>
/// AI単体を処理するクラス
/// </summary>
namespace AI
{
    public class AIAgent : MonoBehaviour
    {
        /// <summary>
        /// このAIを保持しているPlayer
        /// </summary>
        public PlayerAgent Operator { get; private set; }

        [field: SerializeField] public AIHealth Health { get; private set; }
        [field: SerializeField] public AIAssault Assault { get; private set; }
        [field: SerializeField] public AIBrain Brain { get; private set; }
        [field: SerializeField] public AITravel Travel { get; private set; }
        [field: SerializeField] public AIPerform Perform { get; private set; }

        [field: SerializeField] public AICamera Camera { get; private set; }

        public AIAgent Spawn(PlayerAgent operator_, Vector2Int pos_)
        {
            name = $"AI：{operator_.Index}";
            Operator = operator_;

            Health.Initialize(this);
            Assault.Initialize(this);
            Brain.Initialize(this);
            Travel.Initialize(this, pos_);
            Perform.Initialize(this);

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