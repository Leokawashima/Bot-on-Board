﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// プレイヤーの操作を管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager m_Local { get; private set; }
    public static List<PlayerManager> m_List { get; private set; } = new();

    public NetworkVariable<int> roll = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
        );
    public NetworkVariable<int> OrderIndex = new NetworkVariable<int>(
        -1,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
        );

    void Start()
    {
        if(IsOwner)
        {
            m_Local = this;
            gameObject.AddComponent<PlayerLocalManager>();
        }
        m_List.Add(this);
    }
}
