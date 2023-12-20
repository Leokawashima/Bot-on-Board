using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map.Chip;
using Map.Object;
using Map.Stage;
using Game;
using Bot;

namespace Map
{
    /// <summary>
    /// マップを管理するクラス
    /// </summary>
    public class MapManager : SingletonMonoBehaviour<MapManager>
    {
#if UNITY_EDITOR
        [Header("Gizmos")]
        [SerializeField] private bool m_drawGizmos = true;
        [SerializeField] private bool m_drawChipGizmos = true;
        [SerializeField] private bool m_drawObjectGizmos = true;
#endif

        [field: SerializeField]
        public Transform ChipParent { get; private set; }
        [field: SerializeField]
        public Transform ObjectParent { get; private set; }

        [SerializeField] float m_WaitOnePlaceSecond = 0.05f;

        public MapStage Stage { get; private set; }

        [field: SerializeField] public List<MapObject> MapObjects { get; private set; } = new();

        public static event Action Event_MapCreated;

        public Vector3 Offset { get; private set; }

        private void OnEnable()
        {
            GameManager.Event_Initialize += OnSystemInitialize;
            GameManager.Event_Turn_Finalize += OnGameFinalize;
        }
        private void OnDisable()
        {
            GameManager.Event_Initialize -= OnSystemInitialize;
            GameManager.Event_Turn_Finalize -= OnGameFinalize;
        }

        private void OnSystemInitialize()
        {
            MapCreate();
        }
        private void OnGameFinalize()
        {
            for (int i = 0; i < MapObjects.Count; i++)
            {
                if (false == MapObjects[i].TurnUpdate(this))
                {
                    // falseでUpdate内部から破壊しリストから削除しているためデクリメントしている
                    --i;
                }
            }
        }

        private void MapCreate()
        {
            Stage = new(MapTable.Stage.Table[0]);
            Offset = new(-Stage.Size.x / 2.0f + 0.5f, 0, -Stage.Size.y / 2.0f + 0.5f);

            StartCoroutine(CoMapCreate());
        }

        private IEnumerator CoMapCreate()
        {
            var _size = Stage.Size;
            var _chipTable = MapTable.Chip.Table;
            var _objectTable = MapTable.Object.Table;

            for (int i = 0, cnt = _size.x + _size.y; i < cnt; ++i)
            {
                for (int z = 0; z < _size.y; ++z)
                {
                    for (int x = 0; x < _size.x; ++x)
                    {
                        // 斜めに生成するために x + Z
                        if (i == x + z)
                        {
                            int _index = z * _size.x + x;
                            if (Stage.SO.MapChip[_index] != -1)
                            {
                                var _mc = _chipTable[Stage.SO.MapChip[_index]]
                                    .Spawn(new Vector2Int(x, z), this);
                            }
                            if (Stage.SO.MapObject[_index] != -1)
                            {
                                var _mo = _objectTable[Stage.SO.MapObject[_index]]
                                    .Spawn(new Vector2Int(x, z), this);
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(m_WaitOnePlaceSecond);
            }

            Event_MapCreated?.Invoke();
        }

        public MapObject ObjectSpawn(MapObject_SO so_, MapChip chip_)
        {
            return so_.Spawn(chip_.Position, this);
        }

        public void AIHitCheck(Vector2Int pos_, BotAgent bot_)
        {
            if (Stage.Object[pos_.y][pos_.x] != null)
            {                   
                Stage.Object[pos_.y][pos_.x].Hit(bot_);
                Stage.Object[pos_.y][pos_.x].Finalize(this);
            }
        }

        public void AIRideCheck(Vector2Int pos_, BotAgent bot_)
        {
            if (Stage.Chip[pos_.y][pos_.x] != null)
            {
                Stage.Chip[pos_.y][pos_.x].Ride(bot_);
            }
        }

        #region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (false ==  m_drawGizmos) return;

            if (m_drawChipGizmos)
            {
                if (Stage != null)
                {
                    var _size = new Vector3(Stage.Size.x, 1, Stage.Size.y);
                    Gizmos.DrawWireCube(transform.position, _size);
                }
            }

            if (m_drawObjectGizmos)
            {
                if (Stage != null)
                {
                    var _offset = Offset + Vector3.up;
                    //二重ループなのでちょっと重い
                    for (int y = 0; y < Stage.Size.y; ++y)
                    {
                        for (int x = 0; x < Stage.Size.x; ++x)
                        {
                            if (Stage.Object[y][x] != null)
                            {
                                Gizmos.color = Color.HSVToRGB(Stage.Object[y][x].Data.Cost / 36.0f % 1, 1, 1);
                                Gizmos.DrawWireCube(transform.position + _offset + new Vector3(x, 0, y), Vector3.one);
                            }
                        }
                    }
                }
            }
        }
#endif
        #endregion Gizmos
    }
}