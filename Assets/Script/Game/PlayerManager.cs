using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// プレイヤーの操作を管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class PlayerManager : NetworkBehaviour
{
    void OnEnable()
    {
        PlayerInputManager.OnMouseClickEvent += OnMouse_Click;
    }
    void OnDisable()
    {
        PlayerInputManager.OnMouseClickEvent -= OnMouse_Click;
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
