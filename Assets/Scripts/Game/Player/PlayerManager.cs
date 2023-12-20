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
        public List<PlayerAgent> Players { get; private set; } = new();

        private const int PLAYER_SIZE = 2;

        public void Initialize()
        {
            for (int i = 0, cnt = PLAYER_SIZE; i < cnt; ++i)
            {
                var _player = Instantiate(m_prefab, transform);
                _player.Initialize(i);
                Players.Add(_player);
            }
        }
    }
}