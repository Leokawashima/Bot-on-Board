using UnityEngine;

namespace Player
{
    public class PlayerAgent : MonoBehaviour
    {
        [field: SerializeField] public int Index { get; private set; } = -1;
        [field: SerializeField] public string Name { get; private set; }

        public void Initialize(int index_)
        {
            Index = index_;
            name = $"Player_{index_ + 1}";
            Name = $"Player_{index_ + 1}";
        }
    }
}