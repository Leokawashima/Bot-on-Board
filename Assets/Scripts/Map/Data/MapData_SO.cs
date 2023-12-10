using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "BoB/Map/MapData")]
public class MapData_SO : ScriptableObject
{
    [field: SerializeField]
    public Vector2Int Size { get; set; } = new Vector2Int(10, 10);
    public int MapSize => Size.x * Size.y;

    // NonSerializeではなくただ隠している
    [HideInInspector] public int[] MapChip;
    [HideInInspector] public int[] MapObject;

    public MapData_SO()
    {
        MapChip = new int[MapSize];
        MapObject = new int[MapSize];
    }
}