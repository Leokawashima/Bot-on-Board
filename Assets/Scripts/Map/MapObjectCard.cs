using System;
using UnityEngine;
using Map;
using Map.Chip;
using Map.Object;

public class MapObjectCard : MonoBehaviour
{
    public MapObject_SO SO;
    public int Index = -1;

    public event Action Event_Trash;

    public MapObject ObjectSpawn(MapChip chip_, MapManager map_)
    {
        return SO.Spawn(chip_.Position, chip_.transform.position + Vector3.up, map_.transform);
    }

    public void Trash()
    {
        Event_Trash?.Invoke();

        Destroy(gameObject);
    }
}