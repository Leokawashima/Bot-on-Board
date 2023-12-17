using UnityEngine;

namespace Map.Stage
{
    [CreateAssetMenu(fileName = "MS_", menuName = "BoB/Map/MapStage")]
    public class MapStage_SO : ScriptableObject
    {
        public string Name = string.Empty;

        [TextArea(3, 5)]
        public string Info = "説明文";

        [HideInInspector]
        public Vector2Int Size = new(10, 10);
        public int MapSize => Size.x * Size.y;

        [HideInInspector] public int[] MapChip;
        [HideInInspector] public int[] MapObject;

        public MapStage_SO()
        {
            MapChip = new int[MapSize];
            MapObject = new int[MapSize];
        }
    }
}