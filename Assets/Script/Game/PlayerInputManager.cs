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
    public static event Action OnMouseSubClickEvent;
    public static event Action OnMouseMiddleClickEvent;
    public static Vector2 mPos { get; private set; }

    void OnEnable()
    {
        var map = new InputActionMapSettings();
        map.Player.MainAction.started += OnAction_MainClick;
        map.Player.SubAction.started += OnAction_SubClick;
        map.Player.SubAction.performed += OnAction_SubClick;
        map.Player.SubAction.canceled += OnAction_SubClick;
        map.Player.MiddleAction.started += OnAction_MiddleClick;
        map.UI.Point.started += OnAction_MouseMove;
        map.UI.Point.performed += OnAction_MouseMove;
        map.UI.Point.canceled += OnAction_MouseMove;
        map.Enable();
    }
    void OnDisable()
    {
        var map = new InputActionMapSettings();
        map.Player.MainAction.started -= OnAction_MainClick;
        map.Player.SubAction.started -= OnAction_SubClick;
        map.Player.SubAction.performed -= OnAction_SubClick;
        map.Player.SubAction.canceled -= OnAction_SubClick;
        map.Player.MiddleAction.started -= OnAction_MiddleClick;
        map.UI.Point.started -= OnAction_MouseMove;
        map.UI.Point.performed -= OnAction_MouseMove;
        map.UI.Point.canceled -= OnAction_MouseMove;
        map.Disable();
    }

    void OnAction_MainClick(InputAction.CallbackContext context)
    {
        OnMouseMainClickEvent?.Invoke();
    }
    void OnAction_SubClick(InputAction.CallbackContext context)
    {
        OnMouseSubClickEvent?.Invoke();
    }
    void OnAction_MiddleClick(InputAction.CallbackContext context)
    {
        OnMouseMiddleClickEvent?.Invoke();
    }

    void OnAction_MouseMove(InputAction.CallbackContext context)
    {
        mPos = context.ReadValue<Vector2>();
    }
}
