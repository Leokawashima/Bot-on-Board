using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapChip_SO_Template : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "null";

    [field: SerializeField] public int Cost { get; private set; } = 0;

    [field: SerializeField] public GameObject Prefab { get; private set; }

    public virtual MapChip MapCreate(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        var _go = Instantiate(Prefab, tf_.position + pos_, Prefab.transform.rotation, tf_);
        var _mc = _go.AddComponent<MapChip>();
        _mc.m_SO = this;

        _mc.m_position = posdata_;

        return _mc;
    }
}