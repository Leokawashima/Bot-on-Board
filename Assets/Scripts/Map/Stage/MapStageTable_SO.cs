using UnityEngine;

namespace Map.Stage
{
    [CreateAssetMenu(fileName = "MSTable", menuName = "BoB/Map/Table/MapStageTable")]
    public class MapStageTable_SO : ScriptableObject
    {
        public MapStage_SO[] Table;
    }
}