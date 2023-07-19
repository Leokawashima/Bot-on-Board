using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using System.Threading.Tasks;

public class SampleCreateAnimation : MonoBehaviour
{
    [Header("Cube")]
    [SerializeField] GameObject cube;
    [Header("Corutine")]
    [SerializeField] Vector3 c_spawnPosition = new Vector3(0, 10, 0);
    [SerializeField] int c_sizeX = 10;
    [SerializeField] int c_sizeZ = 10;
    [SerializeField] int c_cnt = 0;
    [SerializeField] float c_waitsecond = 0.2f;
    [Header("Async/Await")]
    [SerializeField] Vector3 a_spawnPosition = new Vector3(0, 10, 10);
    [SerializeField] int a_sizeX = 10;
    [SerializeField] int a_sizeZ = 10;
    [SerializeField] int a_cnt = 0;
    [SerializeField] int a_waitsecond = 200;

    CancellationTokenSource cts;

    async void Start()
    {
        cts = new CancellationTokenSource();
        var token = cts.Token;
        await MapAnimationAsync(token);
        StartCoroutine(Animation());
    }

    public void OnClick_Cancel()
    {
        cts.Cancel();
    }
    async Task<bool> MapAnimationAsync(CancellationToken token)
    {
        while (true)
        {
            //カウントと同じ場所のみスポーンさせる
            // 0 1 2
            // 1 2 3
            // 2 3 4
            for (int z = 0; z < a_sizeZ; ++z)
            {
                for (int x = 0; x < a_sizeX; ++x)
                {
                    if (a_cnt == z + x)
                        Instantiate(cube, a_spawnPosition + new Vector3(x, 0, z), Quaternion.identity, transform);
                }
            }
            //カウントを増加
            a_cnt++;

            //カウントがマップサイズを超えたら停止
            if (a_cnt == a_sizeX + a_sizeZ)
                break;

            //一定時間待つ
            try
            {
                await Task.Delay(a_waitsecond, token);
            }
            catch(TaskCanceledException)
            {
                Debug.Log("<color=red>Cancel</color>");
                break;
            }
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
            for (int z = 0; z < c_sizeZ; ++z)
            {
                for (int x = 0; x < c_sizeX; ++x)
                {
                    if (c_cnt == z + x)
                        Instantiate(cube, c_spawnPosition + new Vector3(x, 0, z), Quaternion.identity, transform);
                }
            }
            //カウントを増加
            c_cnt++;

            //カウントがマップサイズを超えたら停止
            if (c_cnt == c_sizeX + c_sizeZ)
                break;

            //一定時間待つ
            yield return new WaitForSeconds(c_waitsecond);
        }
    }
}