using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapChip_SO_Template : ScriptableObject
{
    public string m_ChipName = "null";

    public GameObject m_Prefab;
    private Animator m_Animator;
    protected Animator GetAnimator {
        get {
            return m_Animator ? m_Animator = m_Prefab.GetComponent<Animator>() : m_Animator;
        }
    }

    const int MapOffset = 0;

    public virtual MapChip MapCreate(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        var _go = Instantiate(m_Prefab, tf_.position + pos_, m_Prefab.transform.rotation, tf_);
        var _mc = _go.AddComponent<MapChip>();
        _mc.m_SO = this;

        _mc.m_position = posdata_;

        return _mc;
    }
}