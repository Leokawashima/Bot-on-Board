using UnityEngine;
using Map.Chip;
using Map.Object;

namespace Map.Stage
{
    /// <summary>
    /// マップのステートを管理するクラス
    /// </summary>
    public class MapStage
    {
        public readonly MapStage_SO SO;
        public readonly Vector2Int Size;

        public MapChip[][] Chip { get; private set; }
        public MapObject[][] Object { get; private set; }

        public MapStage(MapStage_SO stage_)
        {
            SO = stage_;
            Size = stage_.Size;

            Chip = new MapChip[Size.y][];
            Object = new MapObject[Size.y][];

            for(int i = 0, len = Size.y; i < len; ++i)
            {
                Chip[i] = new MapChip[Size.x];
                Object[i] = new MapObject[Size.x];
            }
        }

        public void SetMapChip(Vector2Int pos_, MapChip mapChip_)
        {
            Chip[pos_.y][pos_.x] = mapChip_;
        }
        public void ResetMapChip(Vector2Int pos_)
        {
            Chip[pos_.y][pos_.x] = null;
        }

        public void SetMapObject(Vector2Int pos_, MapObject mapObject_)
        {
            Object[pos_.y][pos_.x] = mapObject_;
        }
        public void ReSetMapObject(Vector2Int pos_)
        {
            Object[pos_.y][pos_.x] = null;
        }
    }
}