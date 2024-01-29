using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameMode;

public class ModeChoiceManager : SingletonMonoBehaviour<ModeChoiceManager>
{
    [SerializeField] private Canvas m_canvas;

    [SerializeField] private Button m_tutorialButton;
    [SerializeField] private Button m_localButton;
    [SerializeField] private Button m_multiButton;
    [SerializeField] private TMP_Text[] m_dtailText;
    [SerializeField] private Button m_acceptButton;
    [SerializeField] private FadePanelSystem m_fadeSystem;

    [field: SerializeField] public GameModeManager.GameMode GameMode { get; private set; }

    public void Enable() => m_canvas.enabled = true;
    public void Disable() => m_canvas.enabled = false;

    public void Initialize()
    {
        foreach(var text in m_dtailText)
        {
            text.enabled = false;
        }

        m_tutorialButton.onClick.AddListener(OnButtonTutorial);
        m_localButton.onClick.AddListener(OnButtonLocal);
        m_multiButton.onClick.AddListener(OnButtonMulti);

        m_acceptButton.onClick.AddListener(OnButtonAccept);
    }

    private void OnButtonTutorial()
    {
        GameMode = GameModeManager.GameMode.Tutorial;
        m_dtailText[0].enabled = true;
        m_dtailText[1].enabled = false;
        m_dtailText[2].enabled = false;
    }
    private void OnButtonLocal()
    {
        GameMode = GameModeManager.GameMode.Local;
        m_dtailText[1].enabled = true;
        m_dtailText[0].enabled = false;
        m_dtailText[2].enabled = false;
    }
    private void OnButtonMulti()
    {
        GameMode = GameModeManager.GameMode.Multi;
        m_dtailText[2].enabled = true;
        m_dtailText[0].enabled = false;
        m_dtailText[1].enabled = false;
    }

    private void OnButtonAccept()
    {
        switch (GameMode)
        {
            case GameModeManager.GameMode.Non:
                break;
            case GameModeManager.GameMode.Tutorial:
                var _settings = new PlayerSetting[InfoScrollViewManager.Singleton.Infos.Count];
                for (int i = 0; i < _settings.Length; ++i)
                {
                    _settings[i] = InfoScrollViewManager.Singleton.Infos[i].Data;
                }
                GameModeManager.Initialize(GameMode, 10, 15, _settings);
                Initiate.Fade(Name.Scene.Game, Name.Scene.GameMode, Color.black, 1.0f);
                break;
            case GameModeManager.GameMode.Local:
                m_fadeSystem.Event_FadeInCompleted += OnFadeIn;
                m_fadeSystem.Fade();
                break;
            case GameModeManager.GameMode.Multi:
                m_fadeSystem.Event_FadeInCompleted += OnFadeIn;
                break;
        }
        void OnFadeIn()
        {
            Disable();
            GameModeManager.Singleton.Enable();
            m_fadeSystem.Event_FadeInCompleted -= OnFadeIn;
        }
    }
}