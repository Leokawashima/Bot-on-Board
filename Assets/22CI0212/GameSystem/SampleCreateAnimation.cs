using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using System.Threading.Tasks;

public class SampleCreateAnimation : MonoBehaviour
{
    [SerializeField] GameObject cube;
    [SerializeField] Vector3 spawnPosition = new Vector3(0, 10, 0);
    [SerializeField] int sizeX = 10;
    [SerializeField] int sizeZ = 10;
    [SerializeField] int cnt = 0;
    [SerializeField] float waitsecond = 0.2f;
    [SerializeField] int waitmilisecond = 200;
    void Start()
    {
        StartCoroutine(Animation());
    }

    async Task<bool> MapAnimation(CancellationToken token)
    {
        while (true)
        {
            //カウントと同じ場所のみスポーンさせる
            // 0 1 2
            // 1 2 3
            // 2 3 4
            for (int z = 0; z < sizeZ; ++z)
            {
                for (int x = 0; x < sizeX; ++x)
                {
                    if (cnt == z + x)
                        Instantiate(cube, spawnPosition + new Vector3(x, 0, z), Quaternion.identity, transform);
                }
            }
            //カウントを増加
            cnt++;

            //カウントがマップサイズを超えたら停止
            if (cnt == sizeX + sizeZ)
                break;

            //一定時間待つ
            await Task.Delay(waitmilisecond);
        }
        return true;
    }
    IEnumerator Animation()
    {
        while (true)
        {
            //カウントと同じ場所のみスポーンさせる
            // 0 1 2
            // 1 2 3
            // 2 3 4
            for (int z = 0; z < sizeZ; ++z)
            {
                for (int x = 0; x < sizeX; ++x)
                {
                    if (cnt == z + x)
                        Instantiate(cube, spawnPosition + new Vector3(x, 0, z), Quaternion.identity, transform);
                }
            }
            //カウントを増加
            cnt++;

            //カウントがマップサイズを超えたら停止
            if (cnt == sizeX + sizeZ)
                break;

            //一定時間待つ
            yield return new WaitForSeconds(waitsecond);
        }
    }
}