using System;
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
    [SerializeField] bool m_drawGizmos = true;
    [SerializeField] bool m_drawChipGizmos = true;
    [SerializeField] bool m_drawObjectGizmos = true;
#endif
    public static MapManager Singleton { get; private set; }

    [SerializeField] MapData_SO m_MapDataSO;
    public Vector2Int MapDataSize => m_MapDataSO.Size;

    [SerializeField] MapChipTable_SO m_MapChipTable;
    [SerializeField] MapObjectTable_SO m_MapObjectTable;

    [SerializeField] float m_WaitOnePlaceSecond = 0.05f;
    public List<AISystem> m_AIManagerList { get; private set; } = new();

    public MapStateManager MapState { get; private set; }

    [field: SerializeField] public List<MapObject> MapObjectList { get; private set; } = new();

    public static event Action Event_MapCreated;

    public Vector3 Offset => new Vector3(-m_MapDataSO.Size.x / 2.0f + 0.5f, 0, -m_MapDataSO.Size.y / 2.0f + 0.5f) + transform.position;

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

    private void Awake()
    {
        Singleton ??= this;
    }
    private void OnDestroy()
    {
        Singleton = null;
    }

    void MapCreate()
    {
        MapState = new(m_MapDataSO.Size);

        IEnumerator CoMapCreate()
        {
            var _mapOffset = Offset;
            var _cnt = 0;

            while(true)
            {
                for(int z = 0; z < m_MapDataSO.Size.y; ++z)
                {
                    for(int x = 0; x < m_MapDataSO.Size.x; ++x)
                    {
                        if(_cnt == x + z)// 斜めに順生成するための判定
                        {
                            int _index = z * m_MapDataSO.Size.x + x;
                            if(m_MapDataSO.MapChip[_index] != -1)
                            {
                                var _pos = new Vector3(x, 0, z) + _mapOffset;
                                var mc = m_MapChipTable.Table[m_MapDataSO.MapChip[_index]]
                                    .Spawn(new Vector2Int(x, z), _pos, transform);
                                //mc.Initialize(this);
                            }
                            if(m_MapDataSO.MapObject[_index] != -1)
                            {
                                var _pos = new Vector3(x, 0, z) + _mapOffset + Vector3.up;
                                var _mo = m_MapObjectTable.m_Table[m_MapDataSO.MapObject[_index]]
                                    .Spawn(new Vector2Int(z, x), _pos, transform);
                                _mo.Initialize(this);
                            }
                        }
                    }
                }
                _cnt++;// ++_cntで下とまとめても良いが個人的に読み返すときに見落とされがちなので使用しない
                       // 頂点位置計算等の数学フィジカルごり押しプログラムなら良いと思う

                if(_cnt == m_MapDataSO.Size.x + m_MapDataSO.Size.y) break;

                yield return new WaitForSeconds(m_WaitOnePlaceSecond);
            }

            Event_MapCreated?.Invoke();
        }

        StartCoroutine(CoMapCreate());
    }
    public void AIHitObject(Vector2Int pos_, AISystem _ai)
    {
        if (MapState.MapObjectState[pos_.y, pos_.x] != -1)
        {
            // foreachじゃないのはループ中に要素を削除することができないから
            for (int i = 0; i < MapObjectList.Count; ++i)
            {
                if (MapObjectList[i].Position == pos_)
                {
                    // 本来こんな書き方でアイテムの効果を出すわけがないが、
                    // オブジェクト志向の組み方をしている暇がないので一旦回復アイテムはこれ
                    if (MapState.MapObjectState[pos_.y, pos_.x] == -50)
                    {
                        _ai.HealHP(3);
                    }
                    else if (MapState.MapObjectState[pos_.y, pos_.x] == -100)
                    {
                        _ai.m_PowWeapon = 3;
                        _ai.m_UseWeapon = 2;
                    }
                    else if (MapState.MapObjectState[pos_.y, pos_.x] == 50)
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

    #region Gizmos
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(!m_drawGizmos) return;

        if (m_drawChipGizmos)
        {
            var _size = new Vector3(m_MapDataSO.Size.x, 1, m_MapDataSO.Size.y);
            Gizmos.DrawWireCube(transform.position, _size);
        }

        if (m_drawObjectGizmos)
        {
            if (MapState != null)
            {
                var _offset = new Vector3(-m_MapDataSO.Size.x / 2.0f + 0.5f, 1, -m_MapDataSO.Size.y / 2.0f + 0.5f);
                //二重ループなのでちょっと重い
                for(int y = 0; y < m_MapDataSO.Size.y; ++y)
                {
                    for(int x = 0; x < m_MapDataSO.Size.x; ++x)
                    {
                        if (MapState.MapObjectState[y, x] != -1)
                        {
                            Gizmos.color = Color.HSVToRGB(MapState.MapObjectState[y, x] / 36.0f % 1, 1, 1);
                            Gizmos.DrawWireCube(transform.position + _offset + new Vector3(x, 0, y), Vector3.one);
                        }
                    }
                }
            }
            else
            {
                var _size = new Vector3(m_MapDataSO.Size.x, 1, m_MapDataSO.Size.y);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position + Vector3.up, _size);
            }
        }
    }
#endif
    #endregion Gizmos
}