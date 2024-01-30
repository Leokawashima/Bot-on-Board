using GameMode;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        [SerializeField] PlayerAgent m_prefab;

#if UNITY_EDITOR
        [field: Header("Debug")]
        [SerializeField, Range(2, 4)] private int m_playerCount = 2;
        private readonly float[] m_COLORS = new float[]
        {
            0.0f,
            240.0f / 360.0f,
            120.0f / 360.0f,
            60.0f / 360.0f,
        };
        [field: SerializeField]
#endif
        public List<PlayerAgent> Players { get; private set; } = new();

        public void Initialize()
        {
#if UNITY_EDITOR
            var _settings = GameModeManager.PlayerSettings;
            if (_settings == null )
            {
                _settings = new PlayerSetting[m_playerCount];
                for (int i = 0; i < m_playerCount; ++i)
                {
                    _settings[i] = new PlayerSetting();
                    _settings[i].Initialize(i, "ナナシ", m_COLORS[i], 1, 1);
                }
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