using UnityEngine;
using UnityEngine.UI;

namespace GameMode
{
    public class GameModeManager : MonoBehaviour
    {
        [SerializeField] private Button
            m_deckButton,
            m_tutorialButton,
            m_localButton,
            m_multiButton;

        private void Start()
        {
            m_deckButton.onClick.AddListener(OnButtonDeck);
            m_tutorialButton.onClick.AddListener(OnButtonTutorial);
            m_localButton.onClick.AddListener(OnButtonLocal);
            m_multiButton.onClick.AddListener(OnButtonMulti);
        }

        private void OnButtonDeck()
        {
            Initiate.Fade(Name.Scene.Deck, Name.Scene.GameMode, Color.black, 1.0f);
        }
        private void OnButtonTutorial()
        {

        }
        private void OnButtonLocal()
        {
            Initiate.Fade(Name.Scene.Game, Name.Scene.GameMode, Color.black, 1.0f);
        }
        private void OnButtonMulti()
        {

        }
    }
}