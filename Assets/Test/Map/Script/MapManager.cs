using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public enum MapState { Non = 0, Ground = 1, Damage = 2, }
    public enum ObjState { Non = 0, Box = 1, Unko = 2,}

    [SerializeField] Data_SO data_SO;
    [SerializeField] GameObject[] mapChip;
    [SerializeField] GameObject[] objChip;

    public int[,] mapStates { get; private set; }
    public int[,] objStates { get; private set; }

    void Start()
    {
        mapStates = new int[data_SO.y, data_SO.x];
        objStates = new int[data_SO.y, data_SO.x];

        var offset = new Vector3(0.5f, 0, 0.5f);

        for (int y = 0; y < data_SO.y; ++y)
        {
            int yy = y;
            y = data_SO.y - y - 1;
            for (int x = 0; x < data_SO.x; ++x)
            {
                if (data_SO.mapChip[y * data_SO.x + x] != 0)
                {
                    var mapPos = new Vector3(x, 0, y) + offset;
                    Instantiate(mapChip[data_SO.mapChip[y * data_SO.x + x]], mapPos, Quaternion.identity, transform);
                    mapStates[y, x] = data_SO.mapChip[y * data_SO.x + x];
                }
                if (data_SO.objChip[y * data_SO.x + x] != 0)
                {
                    var objPos = new Vector3(x, 0, y) + offset + Vector3.up;
                    Instantiate(objChip[data_SO.objChip[y * data_SO.x + x]], objPos, Quaternion.identity, transform);
                    objStates[y, x] = data_SO.objChip[y * data_SO.x + x];
                }
            }
            y = yy;
        }
    }

    void OnDrawGizmos()
    {
        var pos = new Vector3(data_SO.x, 0, data_SO.y);
        var offset = new Vector3(1, 0, 1);
        var size = new Vector3(data_SO.x, 1, data_SO.y);
        Gizmos.DrawWireCube(transform.position + pos / 2.0f, size);
    }
}