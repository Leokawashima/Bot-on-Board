using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapChip_SO_Template : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "null";

    [field: SerializeField] public int Cost { get; private set; } = 0;

    [field: SerializeField] public MapChip Prefab { get; private set; }

    public virtual MapChip Spawn(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        var _go = Instantiate(Prefab, pos_, Prefab.transform.rotation, tf_);
        _go.m_SO = this;

        _go.m_position = posdata_;

        return _go;
    }
}