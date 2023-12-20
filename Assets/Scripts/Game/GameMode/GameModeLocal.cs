using Unity.Netcode;
using UnityEngine;
using Player;

namespace Game
{
    public class GameModeLocal : GameMode_Template
    {
        public override void Initialize()
        {
            var _playerManager = PlayerManager.Singleton;
            _playerManager.Initialize();

            Destroy(NetworkManager.Singleton.gameObject);
            Destroy(NetConnectManager.Singleton);

            _playerManager.gameObject.AddComponent<PlayerInputManager>();
            _playerManager.gameObject.AddComponent<LocalPlayerManager>();
        }
    }
}