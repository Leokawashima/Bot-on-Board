using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの入力を管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class PlayerInputManager : MonoBehaviour
{
    public static event Action OnMouseMainClickEvent;
    public static event Action OnDragStartEvent;
    public static event Action OnDragCancelEvent;
    public static event Action OnMouseMovePerformEvent;
    public static Vector2 m_Pos { get; private set; }

    InputActionMapSettings m_Map;

    void OnEnable()
    {
        m_Map = new();
        m_Map.Player.MainAction.performed += OnAction_MainClick;
        m_Map.Player.HoldAction.started += OnAction_DragStart;
        m_Map.Player.HoldAction.canceled += OnAction_DragCancel;
        m_Map.Player.Position.started += OnAction_Position;
        m_Map.Player.Position.performed += OnAction_Position;
        m_Map.Player.Position.canceled += OnAction_Position;
        m_Map.Enable();
    }
    void OnDisable()
    {
        m_Map.Player.MainAction.performed -= OnAction_MainClick;
        m_Map.Player.HoldAction.started -= OnAction_DragStart;
        m_Map.Player.HoldAction.canceled -= OnAction_DragCancel;
        m_Map.Player.Position.started -= OnAction_Position;
        m_Map.Player.Position.performed -= OnAction_Position;
        m_Map.Player.Position.canceled -= OnAction_Position;
        m_Map.Disable();
    }

    void OnAction_MainClick(InputAction.CallbackContext context)
    {
        OnMouseMainClickEvent?.Invoke();
    }
    void OnAction_DragStart(InputAction.CallbackContext context)
    {
        OnDragStartEvent?.Invoke();
    }
    void OnAction_DragCancel(InputAction.CallbackContext context)
    {
        OnDragCancelEvent?.Invoke();
    }
    void OnAction_Position(InputAction.CallbackContext context)
    {
        m_Pos = context.ReadValue<Vector2>();
        OnMouseMovePerformEvent?.Invoke();
    }
}