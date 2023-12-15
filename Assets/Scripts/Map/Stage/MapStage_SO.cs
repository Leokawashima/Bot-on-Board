using UnityEngine;

namespace Map.Stage
{
    [CreateAssetMenu(fileName = "MS_", menuName = "BoB/Map/MapStage")]
    public class MapStage_SO : ScriptableObject
    {
        [field: SerializeField]
        public Vector2Int Size { get; set; } = new Vector2Int(10, 10);
        public int MapSize => Size.x * Size.y;

        // NonSerializeではなくただ隠している
        [HideInInspector] public int[] MapChip;
        [HideInInspector] public int[] MapObject;

        public MapStage_SO()
        {
            MapChip = new int[MapSize];
            MapObject = new int[MapSize];
        }
    }
}