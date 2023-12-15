using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map.Chip;
using Map.Object;
using Map.Stage;

namespace Map
{
    /// <summary>
    /// マップを管理するクラス
    /// </summary>
    public class MapManager : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Gizmos")]
        [SerializeField] private bool m_drawGizmos = true;
        [SerializeField] private bool m_drawChipGizmos = true;
        [SerializeField] private bool m_drawObjectGizmos = true;
#endif
        public static MapManager Singleton { get; private set; }

        [SerializeField] private Transform
            m_mapChipParent,
            m_mapObjectParent;

        [SerializeField] MapStage_SO m_MapStageSO;
        public Vector2Int MapStageSize => m_MapStageSO.Size;

        [SerializeField] MapChipTable_SO m_MapChipTable;
        [SerializeField] MapObjectTable_SO m_MapObjectTable;

        [SerializeField] float m_WaitOnePlaceSecond = 0.05f;
        public List<AISystem> AIManagerList { get; private set; } = new();

        public MapStateManager MapState { get; private set; }

        [field: SerializeField] public List<MapObject> MapObjectList { get; private set; } = new();

        public static event Action Event_MapCreated;

        public Vector3 Offset => new(-m_MapStageSO.Size.x / 2.0f + 0.5f, 0, -m_MapStageSO.Size.y / 2.0f + 0.5f);

        void OnEnable()
        {
            GameManager.Event_Initialize += OnSystemInitialize;
            GameManager.Event_Turn_Finalize += OnGameFinalize;
        }
        void OnDisable()
        {
            GameManager.Event_Initialize -= OnSystemInitialize;
            GameManager.Event_Turn_Finalize -= OnGameFinalize;
        }

        void OnSystemInitialize()
        {
            MapCreate();
        }
        void OnGameFinalize()
        {
            for (int i = 0; i < MapObjectList.Count; i++)
            {
                if (false == MapObjectList[i].TurnUpdate(this))
                {
                    // falseでUpdate内部から破壊しリストから削除しているためデクリメントしている
                    --i;
                }
            }
        }

        private void Awake()
        {
            Singleton = this;
        }
        private void OnDestroy()
        {
            Singleton = null;
        }

        void MapCreate()
        {
            MapState = new(m_MapStageSO);

            IEnumerator Co_MapCreate()
            {
                var _mapOffset = Offset;
                var _cnt = 0;

                while (true)
                {
                    for (int z = 0; z < m_MapStageSO.Size.y; ++z)
                    {
                        for (int x = 0; x < m_MapStageSO.Size.x; ++x)
                        {
                            if (_cnt == x + z)// 斜めに順生成するための判定
                            {
                                int _index = z * m_MapStageSO.Size.x + x;
                                if (m_MapStageSO.MapChip[_index] != -1)
                                {
                                    var _pos = new Vector3(x, 0, z) + _mapOffset;
                                    var _mc = m_MapChipTable.Table[m_MapStageSO.MapChip[_index]]
                                        .Spawn(new Vector2Int(x, z), _pos, m_mapChipParent);
                                    _mc.Initialize(this);
                                }
                                if (m_MapStageSO.MapObject[_index] != -1)
                                {
                                    var _pos = new Vector3(x, 0, z) + _mapOffset + Vector3.up;
                                    var _mo = m_MapObjectTable.Data[m_MapStageSO.MapObject[_index]]
                                        .Spawn(new Vector2Int(x, z), _pos, m_mapObjectParent);
                                    _mo.Initialize(this);
                                }
                            }
                        }
                    }

                    ++_cnt;

                    if (_cnt == m_MapStageSO.Size.x + m_MapStageSO.Size.y) break;

                    yield return new WaitForSeconds(m_WaitOnePlaceSecond);
                }

                Event_MapCreated?.Invoke();
            }

            StartCoroutine(Co_MapCreate());
        }

        public MapObject ObjectSpawn(MapObject_SO so_, MapChip chip_)
        {
            return so_.Spawn(chip_.Position, chip_.transform.position + Vector3.up, m_mapObjectParent);
        }

        public void AIHitObject(Vector2Int pos_, AISystem ai_)
        {
            if (MapState.MapObjects[pos_.y][pos_.x] != null)
            {
                // ループ中に要素を削除する可能性があるためfor
                for (int i = 0; i < MapObjectList.Count; ++i)
                {
                    if (MapObjectList[i].Position == pos_)
                    {
                        MapObjectList[i].Hit(ai_);
                        MapObjectList[i--].Finalize(this);
                        // i--;を別の行に記述すると不必要な代入と文句を言われるので上に統合
                        break;
                    }
                }
            }
        }

        public void AIRideChip(Vector2Int pos_, AISystem ai_)
        {
            if(MapState.MapChips[pos_.y][pos_.x] != null)
            {
                MapState.MapChips[pos_.y][pos_.x].Ride(ai_);
            }
        }

        #region Gizmos
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!m_drawGizmos) return;

            if (m_drawChipGizmos)
            {
                var _size = new Vector3(m_MapStageSO.Size.x, 1, m_MapStageSO.Size.y);
                Gizmos.DrawWireCube(transform.position, _size);
            }

            if (m_drawObjectGizmos)
            {
                if (MapState != null)
                {
                    var _offset = new Vector3(-m_MapStageSO.Size.x / 2.0f + 0.5f, 1, -m_MapStageSO.Size.y / 2.0f + 0.5f);
                    //二重ループなのでちょっと重い
                    for (int y = 0; y < m_MapStageSO.Size.y; ++y)
                    {
                        for (int x = 0; x < m_MapStageSO.Size.x; ++x)
                        {
                            if (MapState.MapObjects[y][x] != null)
                            {
                                Gizmos.color = Color.HSVToRGB(MapState.MapObjects[y][x].Data.Cost / 36.0f % 1, 1, 1);
                                Gizmos.DrawWireCube(transform.position + _offset + new Vector3(x, 0, y), Vector3.one);
                            }
                        }
                    }
                }
                else
                {
                    var _size = new Vector3(m_MapStageSO.Size.x, 1, m_MapStageSO.Size.y);
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(transform.position + Vector3.up, _size);
                }
            }
        }
#endif
        #endregion Gizmos
    }
}