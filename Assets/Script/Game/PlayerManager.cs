using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

/// <summary>
/// プレイヤーの操作を管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class PlayerManager : NetworkBehaviour
{
    Vector2 input;
    Vector2 move;

    void OnEnable()
    {
        PlayerInputManager.OnMouseClickEvent += OnMouse_Click;
        var map = new InputActionMapSettings();
        map.Player.Move.performed += OnMove;
        map.Player.Move.canceled += OnMove;
        map.Enable();
    }
    void OnDisable()
    {
        PlayerInputManager.OnMouseClickEvent -= OnMouse_Click;
        var map = new InputActionMapSettings();
        map.Player.Move.performed -= OnMove;
        map.Player.Move.canceled -= OnMove;
        map.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    //送信バッファ(名前は固定らしい)
    [ServerRpc]
    void SetMoveInputServerRpc(Vector2 input_)
    {
        move = input_;
    }

    private void Update()
    {
        if (IsOwner)
        {
            SetMoveInputServerRpc(input);
        }

        if (IsServer)
        {
            ServerUpdate();
        }
    }

    void ServerUpdate()
    {
        transform.position += (new Vector3(move.x * 0.1f, 0, move.y * 0.1f));
    }

    void OnMouse_Click()
    {
        Ray ray = Camera.main.ScreenPointToRay(PlayerInputManager.mPos);

        if (Physics.Raycast(ray, out var hit))
        {
            hit.collider.gameObject.layer = 10;
            var mat = hit.collider.gameObject.GetComponent<Renderer>().material;
            float r = Random.Range(0.0f, 1.0f), g = Random.Range(0.0f, 1.0f), b = Random.Range(0.0f, 1.0f);
            mat.color = new Color(r, g, b, 1);
        }
    }
}
