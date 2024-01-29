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

        public void Initialize(PlayerSetting setting_)
        {
            Index = setting_.Index;
            name = $"Player_{setting_.Index + 1}:{setting_.Name}";
            Name = $"{setting_.Name}";
            Bots = new(setting_.BotOperations);
        }
    }
}