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

    int cnt = 0;
    Vector3 offset { get { return new Vector3(0.5f, 0, 0.5f); } }
    int waitSecond = 200;
    float AnimY = 10;

    public async Task<bool> MapCreateAsync(CancellationToken token)
    {
        mapStates = new int[data_SO.y, data_SO.x];
        objStates = new int[data_SO.y, data_SO.x];

        while(true)
        {
            for(int z = 0; z < data_SO.y; ++z)
            {
                int zz = z;
                z = data_SO.y - z - 1;
                for(int x = 0; x < data_SO.x; ++x)
                {
                    if (cnt == x + z)
                    {
                        if(data_SO.mapChip[z * data_SO.x + x] != 0)
                        {
                            var mapPos = new Vector3(x, AnimY, z) + offset;
                            Instantiate(mapChip[data_SO.mapChip[z * data_SO.x + x]], mapPos, Quaternion.identity, transform);
                            mapStates[z, x] = data_SO.mapChip[z * data_SO.x + x];
                        }
                        if(data_SO.objChip[z * data_SO.x + x] != 0)
                        {
                            var objPos = new Vector3(x, AnimY, z) + offset + Vector3.up;
                            Instantiate(objChip[data_SO.objChip[z * data_SO.x + x]], objPos, Quaternion.identity, transform);
                            objStates[z, x] = data_SO.objChip[z * data_SO.x + x];
                        }
                    }
                }
                z = zz;
            }
            cnt++;

            if(cnt == data_SO.x + data_SO.y)
                break;

            try
            {
                await Task.Delay(waitSecond, token);
            }
            catch(TaskCanceledException)
            {
                return false;
            }
        }
        try
        {
            await Task.Delay(1000, token);
        }
        catch (TaskCanceledException)
        {
            return false;
        }

        return true;
    }

    void OnDrawGizmos()
    {
        var pos = new Vector3(data_SO.x, 0, data_SO.y);
        var size = new Vector3(data_SO.x, 1, data_SO.y);
        Gizmos.DrawWireCube(transform.position + pos / 2.0f, size);
    }
}