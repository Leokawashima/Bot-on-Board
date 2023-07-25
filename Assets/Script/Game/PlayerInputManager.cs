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
    public static event Action OnMouseClickEvent;
    public static Vector2 mPos { get; private set; }

    void OnEnable()
    {
        var map = new InputActionMapSettings();
        map.Player.Fire.started += OnAction_MouseClick;
        map.UI.Point.started += OnAction_MouseMove;
        map.UI.Point.performed += OnAction_MouseMove;
        map.UI.Point.canceled += OnAction_MouseMove;
        map.Enable();
    }
    void OnDisable()
    {
        var map = new InputActionMapSettings();
        map.Player.Fire.started -= OnAction_MouseClick;
        map.UI.Point.started -= OnAction_MouseMove;
        map.UI.Point.performed -= OnAction_MouseMove;
        map.UI.Point.canceled -= OnAction_MouseMove;
        map.Disable();
    }

    void OnAction_MouseClick(InputAction.CallbackContext context)
    {
        OnMouseClickEvent?.Invoke();
    }

    void OnAction_MouseMove(InputAction.CallbackContext context)
    {
        mPos = context.ReadValue<Vector2>();
    }
}
