using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class MapManager : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] bool m_DrawGizmos = true;
    [SerializeField] bool m_DrawMapGizmos = true;
    [SerializeField] bool m_DrawObjGizmos = true;
#endif
    [SerializeField] Data_SO m_Data_SO;
    public Data_SO Data_SO { get { return m_Data_SO; } }
    [SerializeField] GameObject[] m_MapChip;
    [SerializeField] GameObject[] m_ObjChip;

    [SerializeField] float m_WaitOnePlaceSecond = 0.05f;

    public int[,] m_MapStates { get; private set; }
    public int[,] m_ObjStates { get; private set; }

    public event Action OnMapCreated;

    Vector3 Offset { get { return new Vector3(0.5f, 0, 0.5f); } }

    public void MapCreate()
    {
        StartCoroutine(CoMapCreate());
    }

    IEnumerator CoMapCreate()
    {
        m_MapStates = new int[m_Data_SO.y, m_Data_SO.x];
        m_ObjStates = new int[m_Data_SO.y, m_Data_SO.x];
        
        var _mapOffset = new Vector3(-m_Data_SO.y / 2.0f, 0, -m_Data_SO.x / 2.0f) + transform.position;
        var _cnt = 0;

        while(true)
        {
            for(int z = 0; z < m_Data_SO.y; ++z)
            {
                for(int x = 0; x < m_Data_SO.x; ++x)
                {
                    if (_cnt == x + z)
                    {
                        if(m_Data_SO.mapChip[z * m_Data_SO.x + x] != 0)
                        {
                            var _pos = new Vector3(x, 0, z) + Offset + _mapOffset;
                            var _map = Instantiate(m_MapChip[m_Data_SO.mapChip[z * m_Data_SO.x + x]], _pos, Quaternion.identity, transform);
                            m_MapStates[z, x] = m_Data_SO.mapChip[z * m_Data_SO.x + x];
                            var _data = _map.GetComponent<MapChip>();
                            _data.m_IndexX = x;
                            _data.m_IndexY = z;
                        }
                        if(m_Data_SO.objChip[z * m_Data_SO.x + x] != 0)
                        {
                            var _pos = new Vector3(x, 0, z) + Offset + Vector3.up + _mapOffset;
                            Instantiate(m_ObjChip[m_Data_SO.objChip[z * m_Data_SO.x + x]], _pos, Quaternion.identity, transform);
                            m_ObjStates[z, x] = m_Data_SO.objChip[z * m_Data_SO.x + x];
                        }
                    }
                }
            }
            _cnt++;//++_cntで下とまとめても良いが個人的に読み返すときに見落とされがちなので好きでない
                   //頂点位置計算等の数学フィジカルプログラムなら良いけど

            if(_cnt == m_Data_SO.x + m_Data_SO.y) break;

            yield return new WaitForSeconds(m_WaitOnePlaceSecond);
        }

        OnMapCreated?.Invoke();
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(!m_DrawGizmos) return;

        if (m_DrawMapGizmos)
        {
            var _size = new Vector3(m_Data_SO.x, 1, m_Data_SO.y);
            Gizmos.DrawWireCube(transform.position, _size);
        }

        if (m_DrawObjGizmos)
        {
            if (m_ObjStates != null)
            {
                var _offset = new Vector3(-Data_SO.x / 2.0f + 0.5f, 1, -Data_SO.y / 2.0f + 0.5f);
                //二重ループなのでちょっと重い
                for(int y = 0; y < m_Data_SO.y; ++y)
                {
                    for(int x = 0; x < m_Data_SO.x; ++x)
                    {
                        if (m_ObjStates[y, x] != 0)
                        {
                            Gizmos.color = Color.HSVToRGB(m_ObjStates[y, x] / 360.0f % 1, 1, 1);
                            Gizmos.DrawWireCube(transform.position + _offset + new Vector3(x, 0, y), Vector3.one);
                        }
                    }
                }
            }
            else
            {
                var _size = new Vector3(m_Data_SO.x, 1, m_Data_SO.y);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position + Vector3.up, _size);
            }
        }
    }
#endif
}