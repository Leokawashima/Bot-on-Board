using UnityEngine;
using UnityEngine.UI;

public class DeckEditManager : MonoBehaviour
{
    [SerializeField] private Button m_backButton;
    [SerializeField] private Button m_saveButton;

    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}