using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// くそ適当なゲーム終了処理
/// </summary>
public class EscGameManager : MonoBehaviour
{
    [SerializeField] EscMenuManager manager;

    [SerializeField] Button m_button;

    private InputActionMapSettings m_inputMap;

    private void OnEnable()
    {
        m_inputMap = new();
        m_inputMap.UI.Esc.started += OnClickButton;
        m_inputMap.Enable();
    }
    private void OnDisable()
    {
        m_inputMap.UI.Esc.started -= OnClickButton;
        m_inputMap.Disable();
        m_inputMap = null;
    }

    private void Start()
    {
        if (m_button != null)
        {
            m_button.onClick.AddListener(manager.Switch);
        }
    }

    private void OnClickButton(InputAction.CallbackContext context_)
    {
        manager.Switch();
    }
}