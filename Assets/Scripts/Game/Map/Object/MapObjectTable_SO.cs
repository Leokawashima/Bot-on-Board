using UnityEngine;

namespace Map.Object
{
    [CreateAssetMenu(fileName = "MapObjectTable", menuName = "BoB/Map/Table/MapObjectTable")]
    public class MapObjectTable_SO : ScriptableObject
    {
        public MapObject_SO[] Table;
    }
}