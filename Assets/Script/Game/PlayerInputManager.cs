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
    public static event Action OnDragEvent;
    public static event Action OnMouseMiddleClickEvent;
    public static Vector2 m_Pos { get; private set; }

    InputActionMapSettings m_Map;

    void OnEnable()
    {
        m_Map = new();
        m_Map.Player.MainAction.started += OnAction_MainClick;
        m_Map.Player.DragAction.started += OnAction_DragStart;
        m_Map.Player.DragAction.performed += OnAction_Drag;
        m_Map.Player.DragAction.canceled += OnAction_Drag;
        m_Map.Player.MiddleAction.started += OnAction_MiddleClick;
        m_Map.Enable();
    }
    void OnDisable()
    {
        m_Map.Player.MainAction.started -= OnAction_MainClick;
        m_Map.Player.DragAction.started -= OnAction_DragStart;
        m_Map.Player.DragAction.performed -= OnAction_Drag;
        m_Map.Player.DragAction.canceled -= OnAction_Drag;
        m_Map.Player.MiddleAction.started -= OnAction_MiddleClick;
        m_Map.Disable();
    }

    void OnAction_MainClick(InputAction.CallbackContext context)
    {
        OnMouseMainClickEvent?.Invoke();
    }
    void OnAction_DragStart(InputAction.CallbackContext context)
    {
        m_Pos = context.ReadValue<Vector2>();
        OnDragStartEvent?.Invoke();
    }
    void OnAction_Drag(InputAction.CallbackContext context)
    {
        m_Pos = context.ReadValue<Vector2>();
        OnDragEvent?.Invoke();
    }
    void OnAction_MiddleClick(InputAction.CallbackContext context)
    {
        OnMouseMiddleClickEvent?.Invoke();
    }
}
