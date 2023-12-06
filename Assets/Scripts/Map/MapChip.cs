using Map;
using UnityEngine;

public class MapChip : MonoBehaviour
{
    public MapChip_SO_Template m_SO;
    public Vector2Int m_position = Vector2Int.zero;

    public Material Material { get; private set; }

    public void Initialize(MapManager mapManager_)
    {
        Material = GetComponent<Renderer>().material;
    }
}