using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStateManager
{
    // オブジェクトのインデックスを格納する配列　恐らく今後消す
    public int[,] MapChipState { get; private set; }
    public int[,] MapObjectState { get; private set; }
    // コストや当たり判定等を格納するマップ
    public int[,] MapChipCost { get; private set; }
    public int[,] MapObjectCost { get; private set; }
    public bool[,] MapCollisionState { get; private set; }

    public MapStateManager(Vector2Int mapSize_)
    {
        MapChipState = new int[mapSize_.y, mapSize_.x];
        MapObjectState = new int[mapSize_.y,mapSize_.x];

        MapChipCost = new int[mapSize_.y, mapSize_.x];
        MapObjectCost = new int[mapSize_.y,mapSize_.x];
        MapCollisionState = new bool[mapSize_.y, mapSize_.x];

        for(int y = 0; y < mapSize_.y; ++y)
        {
            for(int x = 0; x < mapSize_.x; ++x)
            {
                MapChipState[y, x] = -1;
                MapObjectState[y, x] = -1;

                MapChipCost[y, x] = 0;
                MapObjectCost[y, x] = 0;
                MapCollisionState[y, x] = false;
            }
        }
    }

    public void SetMapChip(Vector2Int pos_, MapChip_SO_Template mapChip_)
    {
        MapChipState[pos_.y, pos_.x] = 1;
        MapChipCost[pos_.y, pos_.x] = mapChip_.Cost;
    }
    public void ResetMapChip(Vector2Int pos_)
    {
        MapChipState[pos_.y, pos_.x] = -1;
        MapChipCost[pos_.y, pos_.x] = 0;
    }

    public void SetMapObject(Vector2Int pos_, MapObject_SO_Template mapObject_)
    {
        MapObjectState[pos_.y, pos_.x] = 1;
        MapObjectCost[pos_.y, pos_.x] = mapObject_.Cost;
        MapCollisionState[pos_.y, pos_.x] = mapObject_.m_IsCollider;
    }

    public void ReSetMapObject(Vector2Int pos_)
    {
        MapObjectState[pos_.y, pos_.x] = -1;
        MapObjectCost[pos_.y, pos_.x] = 0;
        MapCollisionState[pos_.y, pos_.x] = false;
    }
}