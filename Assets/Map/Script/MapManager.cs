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

    public int[,] objStates { get; private set; }

    void Start()
    {
        for (int y = 0; y < data_SO.y; ++y)
        {
            for (int x = 0; x < data_SO.x; ++x)
            {
                if (data_SO.mapChip[y * data_SO.y + y] != 0)
                {
                    var mapPos = new Vector3(x, 0, y);
                    Instantiate(mapChip[data_SO.mapChip[y * data_SO.y + y]], mapPos, Quaternion.identity, transform);
                }
                if(data_SO.objChip[data_SO.mapChip[y * data_SO.y + y]] != 0)
                {
                    var objPos = new Vector3(x, 0, y) + Vector3.up;
                    Instantiate(objChip[data_SO.objChip[y * data_SO.y + y]], objPos, Quaternion.identity, transform);
                }
            }
        }
    }
}