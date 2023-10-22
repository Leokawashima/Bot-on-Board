using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Button m_titleButton;

    void Start()
    {
        m_titleButton.onClick.AddListener(OnButtonTitle);
    }

    void OnButtonTitle()
    {
        Initiate.Fade(Name.Scene.Title, Color.black, 1.0f);
    }
}
