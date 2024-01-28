using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModeChoiceManager : SingletonMonoBehaviour<ModeChoiceManager>
{
    [SerializeField] private Canvas m_canvas;

    [SerializeField] private Button m_tutorialButton;
    [SerializeField] private Button m_localButton;
    [SerializeField] private Button m_multiButton;
    [SerializeField] private TMP_Text[] m_dtailText;
    [SerializeField] private Button m_acceptButton;

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
        GlobalSystem.SetGameMode(GlobalSystem.GameMode.Tutorial);
        m_dtailText[0].enabled = true;
        m_dtailText[1].enabled = false;
        m_dtailText[2].enabled = false;
    }
    private void OnButtonLocal()
    {
        GlobalSystem.SetGameMode(GlobalSystem.GameMode.Local);
        m_dtailText[1].enabled = true;
        m_dtailText[0].enabled = false;
        m_dtailText[2].enabled = false;
    }
    private void OnButtonMulti()
    {
        GlobalSystem.SetGameMode(GlobalSystem.GameMode.Multi);
        m_dtailText[2].enabled = true;
        m_dtailText[0].enabled = false;
        m_dtailText[1].enabled = false;
    }

    private void OnButtonAccept()
    {
        switch (GlobalSystem.CurrentGameMode)
        {
            case GlobalSystem.GameMode.Non:
                break;
            case GlobalSystem.GameMode.Local:
                break;
            case GlobalSystem.GameMode.Tutorial:
                Initiate.Fade(Name.Scene.Game, Name.Scene.GameMode, Color.black, 1.0f);
                break;
            case GlobalSystem.GameMode.Multi:
                break;
        }
    }
}