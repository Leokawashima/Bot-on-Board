using UnityEngine;
using Player;

namespace Game
{
    public class GameModeTutorial : GameMode_Template
    {
        public override void Initialize()
        {
            PlayerManager.Singleton.Initialize();
        }
    }
}