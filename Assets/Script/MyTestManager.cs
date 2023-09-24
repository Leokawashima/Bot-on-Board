using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTestManager : MonoBehaviour
{
    public Vector2Int MapSize = new Vector2Int(10, 10);
    const int w = 10000;
    const int d = 100;
    public int[,] mapCost =
    {
        {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
        {   w,   d,   w,   d,   w,   w,   d,   w,   0,   w },
        {   0,   d,   0,   d,   0,   0,   0,   0,   0,   0 },
        {   w,   d,   w,   d,   w,   w,   d,   w,   0,   w },
        {   0,   d,   0,   d,   0,   0,   0,   0,   0,   0 },
        {   0,   d,   0,   0,   d,   d,   0,   0,   0,   0 },
        {   w,   d,   w,   d,   w,   w,   0,   w,   0,   w },
        {   0,   d,   0,   d,   0,   0,   0,   d,   0,   0 },
        {   w,   d,   w,   d,   w,   w,   0,   w,   d,   w },
        {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 },
    };

    private void OnDrawGizmos()
    {
        for(int y = 0; y < MapSize.y; ++y)
            for(int x = 0; x < MapSize.x; ++x)
            {
                Gizmos.color = Color.HSVToRGB(mapCost[y, x] / 360.0f % 1, 1, 1);
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, y), Vector3.one);
            }
    }
}
