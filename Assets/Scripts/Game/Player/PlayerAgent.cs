using System.Collections.Generic;
using UnityEngine;
using Bot;

namespace Player
{
    public class PlayerAgent : MonoBehaviour
    {
        [field: SerializeField] public int Index { get; private set; } = -1;
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public List<BotAgent> Bots { get; private set; }

        public void Initialize(int index_)
        {
            Index = index_;
            name = $"Player_{index_ + 1}";
            Name = $"Player_{index_ + 1}";
            Bots = new(1);
        }
    }
}