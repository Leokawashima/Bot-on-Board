﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class MapManager : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Gizmos")]
    [SerializeField] bool m_DrawGizmos = true;
    [SerializeField] bool m_DrawMapGizmos = true;
    [SerializeField] bool m_DrawObjGizmos = true;
#endif
    public static MapManager Singleton { get; private set; }

    [Header("Data")]
    [SerializeField] MapData_SO m_Data_SO;
    public MapData_SO Data_SO => m_Data_SO;

    [SerializeField] MapChipTable_SO m_MapChipTable;
    [field: SerializeField] public MapObjectTable_SO m_MapObjectTable { get; private set; }

    [SerializeField] float m_WaitOnePlaceSecond = 0.05f;
    public List<AISystem> m_AIManagerList { get; private set; } = new();

    public int[,] m_MapStates { get; private set; }
    public int[,] m_ObjStates { get; private set; }
    public void SetObjState(Vector2Int pos_, int cost_)
    {
        m_ObjStates[pos_.y, pos_.x] = cost_;
    }
    public bool[,] m_CollisionState { get; private set; }
    public void SetCollisionState(Vector2Int pos_, bool isCollision_)
    {
        m_CollisionState[pos_.y, pos_.x] = isCollision_;
    }

    [field: SerializeField] public List<MapObject> MapObjectList { get; private set; } = new();

    public static event Action Event_MapCreated;

    public Vector3 Offset => new Vector3(-m_Data_SO.x / 2.0f + 0.5f, 0, -m_Data_SO.y / 2.0f + 0.5f) + transform.position;

    void OnEnable()
    {
        GameSystem.Event_Initialize += OnSystemInitialize;
        GameSystem.Event_Turn_Finalize += OnGameFinalize;
    }
    void OnDisable()
    {
        GameSystem.Event_Initialize -= OnSystemInitialize;
        GameSystem.Event_Turn_Finalize -= OnGameFinalize;
    }

    void OnSystemInitialize()
    {
        MapCreate();
    }
    void OnGameFinalize()
    {
        for(int i = 0; i < MapObjectList.Count; i++)
        {
            if (false == MapObjectList[i].ObjectUpdate(this))
            {
                --i;
            }
        }
    }

    void Start()
    {
        Singleton ??= this;
    }
    void OnDestroy()
    {
        Singleton = null;
    }

    void MapCreate()
    {
        IEnumerator CoMapCreate()
        {
            m_MapStates = new int[m_Data_SO.y, m_Data_SO.x];
            m_ObjStates = new int[m_Data_SO.y, m_Data_SO.x];
            m_CollisionState = new bool[m_Data_SO.y, m_Data_SO.x];

            var _mapOffset = Offset;
            var _cnt = 0;

            while(true)
            {
                for(int z = 0; z < m_Data_SO.y; ++z)
                {
                    for(int x = 0; x < m_Data_SO.x; ++x)
                    {
                        if(_cnt == x + z)//斜めに順生成するための判定
                        {
                            int _index = z * m_Data_SO.x + x;
                            if(m_Data_SO.mapChip[_index] != -1)
                            {
                                var _pos = new Vector3(x, 0, z) + _mapOffset;
                                m_MapChipTable.m_Table[m_Data_SO.mapChip[_index]]
                                    .MapCreate(new Vector2Int(x, z), _pos, transform);
                            }
                            if(m_Data_SO.objChip[_index] != -1)
                            {
                                var _pos = new Vector3(x, 0, z) + _mapOffset + Vector3.up;
                                var _mo = m_MapObjectTable.m_Table[m_Data_SO.objChip[_index]]
                                    .ObjectSpawn(new Vector2Int(z, x), _pos, transform);
                                _mo.Initialize(this);
                            }

                            m_MapStates[z, x] = m_Data_SO.mapChip[z * m_Data_SO.x + x];
                            m_ObjStates[z, x] = m_Data_SO.objChip[z * m_Data_SO.x + x];
                        }
                    }
                }
                _cnt++;//++_cntで下とまとめても良いが個人的に読み返すときに見落とされがちなので好きでない
                       //頂点位置計算等の数学フィジカルごり押しプログラムなら良いと思う

                if(_cnt == m_Data_SO.x + m_Data_SO.y) break;

                yield return new WaitForSeconds(m_WaitOnePlaceSecond);
            }

            Event_MapCreated?.Invoke();
        }

        StartCoroutine(CoMapCreate());
    }
    public void AIHitObject(Vector2Int pos_, AISystem _ai)
    {
        if (m_ObjStates[pos_.y, pos_.x] != -1)
        {
            // foreachじゃないのはループ中に要素を削除することができないから
            for (int i = 0; i < MapObjectList.Count; ++i)
            {
                if (MapObjectList[i].m_Pos == pos_)
                {
                    // 本来こんな書き方でアイテムの効果を出すわけがないが、
                    // オブジェクト志向の組み方をしている暇がないので一旦回復アイテムはこれ
                    if (m_ObjStates[pos_.y, pos_.x] == -50)
                    {
                        _ai.HealHP(3);
                    }
                    else if (m_ObjStates[pos_.y, pos_.x] == -100)
                    {
                        _ai.m_PowWeapon = 3;
                        _ai.m_UseWeapon = 2;
                    }
                    else if (m_ObjStates[pos_.y, pos_.x] == 50)
                    {
                        _ai.m_Stan = 1;
                    }
                    MapObjectList[i].ObjectDestroy(this);
                    --i;
                    break;
                }
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(!m_DrawGizmos) return;

        if (m_DrawMapGizmos)
        {
            var _size = new Vector3(m_Data_SO.x, 1, m_Data_SO.y);
            Gizmos.DrawWireCube(transform.position, _size);
        }

        if (m_DrawObjGizmos)
        {
            if (m_ObjStates != null)
            {
                var _offset = new Vector3(-Data_SO.x / 2.0f + 0.5f, 1, -Data_SO.y / 2.0f + 0.5f);
                //二重ループなのでちょっと重い
                for(int y = 0; y < m_Data_SO.y; ++y)
                {
                    for(int x = 0; x < m_Data_SO.x; ++x)
                    {
                        if (m_ObjStates[y, x] != -1)
                        {
                            Gizmos.color = Color.HSVToRGB(m_ObjStates[y, x] / 36.0f % 1, 1, 1);
                            Gizmos.DrawWireCube(transform.position + _offset + new Vector3(x, 0, y), Vector3.one);
                        }
                    }
                }
            }
            else
            {
                var _size = new Vector3(m_Data_SO.x, 1, m_Data_SO.y);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position + Vector3.up, _size);
            }
        }
    }
#endif
}