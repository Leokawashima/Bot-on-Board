using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 入力を管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class InputManager : MonoBehaviour
{
    public static event Action Event_Main;
    public static event Action Event_DragStart;
    public static event Action Event_DragCancel;
    public static event Action Event_Position;
    public static event Action Event_Esc;
    public static event Action Event_Any;

    public static Vector2 Position { get; private set; }

    private InputActionMapSettings m_input;

    private void OnEnable()
    {
        m_input = new();
        m_input.Player.Main.performed += OnMain;
        m_input.Player.Hold.started += OnDragStart;
        m_input.Player.Hold.canceled += OnDragCancel;
        m_input.Player.Position.started += OnPosition;
        m_input.Player.Position.performed += OnPosition;
        m_input.Player.Position.canceled += OnPosition;
        m_input.Player.Esc.started += OnEsc;
        m_input.Player.Any.started += OnAny;
        m_input.Enable();
    }
    private void OnDisable()
    {
        m_input.Player.Main.performed -= OnMain;
        m_input.Player.Hold.started -= OnDragStart;
        m_input.Player.Hold.canceled -= OnDragCancel;
        m_input.Player.Position.started -= OnPosition;
        m_input.Player.Position.performed -= OnPosition;
        m_input.Player.Position.canceled -= OnPosition;
        m_input.Player.Esc.started -= OnEsc;
        m_input.Player.Any.started -= OnAny;
        m_input.Disable();
        m_input = null;
    }

    private void OnMain(InputAction.CallbackContext context_)
    {
        Event_Main?.Invoke();
    }
    private void OnDragStart(InputAction.CallbackContext context_)
    {
        Event_DragStart?.Invoke();
    }
    private void OnDragCancel(InputAction.CallbackContext context_)
    {
        Event_DragCancel?.Invoke();
    }
    private void OnPosition(InputAction.CallbackContext context_)
    {
        Position = context_.ReadValue<Vector2>();
        Event_Position?.Invoke();
    }
    private void OnEsc(InputAction.CallbackContext context_)
    {
        Event_Esc?.Invoke();
    }
    private void OnAny(InputAction.CallbackContext context_)
    {
        Event_Any?.Invoke();
    }
}