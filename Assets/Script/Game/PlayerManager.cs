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
    [SerializeField] GameObject m_Robot;

    MapChip m_SelectChip;

    void OnEnable()
    {
        PlayerInputManager.OnMouseMainClickEvent += OnMouse_MainClick;
    }
    void OnDisable()
    {
        PlayerInputManager.OnMouseMainClickEvent -= OnMouse_MainClick;
    }

    private void Start()
    {
        m_Robot.SetActive(false);
    }

    void OnMouse_MainClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(PlayerInputManager.m_Pos);

        int mask = 1 << Name.Layer.Map; 
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, mask))
        {
            var map = hit.collider.GetComponent<MapChip>();
            if (m_SelectChip != map)
            {
                m_SelectChip?.Stop();
                map.HighLight();
                m_SelectChip = map;
            }
        }
    }
}
