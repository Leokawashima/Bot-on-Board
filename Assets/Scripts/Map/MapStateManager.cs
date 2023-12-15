﻿using UnityEngine;
using Map.Chip;
using Map.Object;
using Map.Stage;

namespace Map
{
    /// <summary>
    /// マップのステートを管理するクラス
    /// </summary>
    public class MapStateManager
    {
        // 増やしたい情報に応じて配列の変数を増やしていくとクソプログラムになってしまうので
        // オブジェクトとチップの配列だけを保持してどのインターフェースを持っているかで管理するようにする
        public Vector2Int MapSize { get; private set; }

        public MapChip[][] MapChips { get; private set; }
        public MapObject[][] MapObjects { get; private set; }

        public MapStateManager(MapStage_SO stage_)
        {
            MapSize = stage_.Size;

            MapChips = new MapChip[MapSize.y][];
            MapObjects = new MapObject[MapSize.y][];

            for(int i = 0; i < MapSize.y; ++i)
            {
                MapChips[i] = new MapChip[MapSize.x];
                MapObjects[i] = new MapObject[MapSize.x];
            }
        }

        public void SetMapChip(Vector2Int pos_, MapChip mapChip_)
        {
            MapChips[pos_.y][pos_.x] = mapChip_;
        }
        public void ResetMapChip(Vector2Int pos_)
        {
            MapChips[pos_.y][pos_.x] = null;
        }

        public void SetMapObject(Vector2Int pos_, MapObject mapObject_)
        {
            MapObjects[pos_.y][pos_.x] = mapObject_;
        }

        public void ReSetMapObject(Vector2Int pos_)
        {
            MapObjects[pos_.y][pos_.x] = null;
        }
    }
}