using GameMode;
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

        public void Initialize()
        {
#if UNITY_EDITOR
            var _settings = GameModeManager.PlayerSettings;
            if (_settings == null )
            {

            }
#else
            var _settings = GameModeDataBase.PlayerSettings;
#endif
            for (int i = 0, cnt = _settings.Length; i < cnt; ++i)
            {
                var _player = Instantiate(m_prefab, transform);
                _player.Initialize(_settings[i]);
                Players.Add(_player);
            }
        }
    }
}