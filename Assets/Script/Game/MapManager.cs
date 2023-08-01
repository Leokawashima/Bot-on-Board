using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// マップを管理するクラス
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class MapManager : MonoBehaviour
{
    public enum MapState { Non = 0, Ground = 1, Damage = 2, }
    public enum ObjState { Non = 0, Box = 1, Unko = 2,}

    [SerializeField] Data_SO data_SO;
    [SerializeField] GameObject[] mapChip;
    [SerializeField] GameObject[] objChip;

    public int[,] mapStates { get; private set; }
    public int[,] objStates { get; private set; }

    public event Action OnMapCreated;

    int cnt = 0;
    Vector3 offset { get { return new Vector3(0.5f, 0, 0.5f); } }
    float waitSecond = 0.2f;
    float AnimY = 10;

    public void MapCreate()
    {
        StartCoroutine(CoMapCreate());
    }

    IEnumerator CoMapCreate()
    {
        mapStates = new int[data_SO.y, data_SO.x];
        objStates = new int[data_SO.y, data_SO.x];
        
        var mapOffset = new Vector3(-data_SO.y / 2.0f, 0, -data_SO.x / 2.0f) + transform.position;

        while(true)
        {
            for(int z = 0; z < data_SO.y; ++z)
            {
                for(int x = 0; x < data_SO.x; ++x)
                {
                    if (cnt == x + z)
                    {
                        if(data_SO.mapChip[z * data_SO.x + x] != 0)
                        {
                            var mapPos = new Vector3(x, AnimY, z) + offset + mapOffset;
                            var map = Instantiate(mapChip[data_SO.mapChip[z * data_SO.x + x]], mapPos, Quaternion.identity, transform);
                            mapStates[z, x] = data_SO.mapChip[z * data_SO.x + x];
                            var data = map.GetComponent<MapChip>();
                            data.m_State = (MapState)Enum.ToObject(typeof(MapState), mapStates[z, x]);
                            data.m_IndexX = x;
                            data.m_IndexY = z;
                        }
                        if(data_SO.objChip[z * data_SO.x + x] != 0)
                        {
                            var objPos = new Vector3(x, AnimY, z) + offset + Vector3.up + mapOffset;
                            Instantiate(objChip[data_SO.objChip[z * data_SO.x + x]], objPos, Quaternion.identity, transform);
                            objStates[z, x] = data_SO.objChip[z * data_SO.x + x];
                        }
                    }
                }
            }
            cnt++;

            if(cnt == data_SO.x + data_SO.y)
                break;

            yield return new WaitForSeconds(waitSecond);
        }

        OnMapCreated?.Invoke();
    }

    void OnDrawGizmos()
    {
        var size = new Vector3(data_SO.x, 1, data_SO.y);
        Gizmos.DrawWireCube(transform.position, size);
    }
}