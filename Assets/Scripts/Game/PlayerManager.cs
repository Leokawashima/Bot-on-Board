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

    public static PlayerManager m_Local { get; private set; }
    public static List<PlayerManager> m_List { get; private set; } = new();

    public NetworkVariable<int> roll = new NetworkVariable<int>(
        0,                                          // 初期値
        NetworkVariableReadPermission.Everyone,     // 読み取り権限
        NetworkVariableWritePermission.Owner        // 書き込み権限
        );
    public NetworkVariable<int> OrderIndex = new NetworkVariable<int>(
        -1,                                         // 初期値
        NetworkVariableReadPermission.Everyone,     // 読み取り権限
        NetworkVariableWritePermission.Owner        // 書き込み権限
        );

    MapChip m_SelectChip;

    void OnEnable()
    {
        PlayerInputManager.OnMouseMainClickEvent += OnMouse_MainClick;
    }
    void OnDisable()
    {
        PlayerInputManager.OnMouseMainClickEvent -= OnMouse_MainClick;
    }

    void Start()
    {
        if(IsOwner) m_Local = this;
        m_List.Add(this);
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
