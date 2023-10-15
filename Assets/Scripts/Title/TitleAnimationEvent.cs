using UnityEngine;

public class TitleAnimationEvent : MonoBehaviour
{
    public void OnGameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}