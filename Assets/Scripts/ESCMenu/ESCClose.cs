using UnityEngine;
using UnityEngine.UI;

public class ESCClose : MonoBehaviour
{
    [SerializeField] private Button m_closebutton;

    private void Awake()
    {
        m_closebutton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        });
    }
}