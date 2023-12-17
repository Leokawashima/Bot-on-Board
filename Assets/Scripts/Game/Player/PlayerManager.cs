using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        [SerializeField] PlayerAgent m_prefab;

#if UNITY_EDITOR
        [field: Header("Debug"), SerializeField]
#endif
        public List<PlayerAgent> PlayerList { get; private set; } = new();

        private const int PLAYER_SIZE = 2;

        public void Initialize()
        {
            for (int i = 0; i < PLAYER_SIZE; ++i)
            {
                var _player = Instantiate(m_prefab, transform);
                _player.Initialize(i);
                PlayerList.Add(_player);
            }
        }
    }
}