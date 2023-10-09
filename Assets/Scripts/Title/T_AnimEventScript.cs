using UnityEngine;

public class T_AnimEventScript : MonoBehaviour
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