using UnityEngine;
using Map.Chip;
using Map.Object;
using Map.Stage;

namespace Map.Table
{
    public static class MapTable
    {
        public static MapStageTable_SO StageTable { get; private set; }
        public static MapChipTable_SO ChipTable { get; private set; }
        public static MapObjectTable_SO ObjectTable { get; private set; }
    }
}