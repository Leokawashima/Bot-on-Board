using UnityEngine;

namespace Map.Chip
{
    [CreateAssetMenu(fileName = "MapChipTable", menuName = "BoB/Map/Table/MapChipTable")]
    public class MapChipTable_SO : ScriptableObject
    {
        public MapChip_SO[] Table;
    }
}