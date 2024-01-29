using Player;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameModeTutorial : GameMode_Template
    {
        public override void Initialize()
        {
            var _playerManager = PlayerManager.Singleton;
            _playerManager.Initialize();

            _playerManager.gameObject.AddComponent<LocalPlayerManager>();

            SceneManager.LoadScene(Name.Scene.Tutorial, LoadSceneMode.Additive);
        }
    }
}