using UnityEngine;

/// <summary>
/// 学校基準を満たすためだけの処理
/// </summary>
public class EscAppQuit : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Event_Esc += OnEsc;
    }
    private void OnDisable()
    {
        InputManager.Event_Esc -= OnEsc;
    }

    private void OnEsc()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}