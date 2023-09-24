using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTest : MonoBehaviour
{
    public Vector2Int pos;
    [SerializeField] MyTestManager testManager;
    [SerializeField] Vector2Int start = new Vector2Int(0, 0);
    [SerializeField] MyTest end;
    [SerializeField] List<Vector2Int> path;

    AStarAlgorithm aStar;

    void Start()
    {
        aStar = new AStarAlgorithm(testManager.MapSize, testManager.mapCost);
        
        StartCoroutine(CoMove());
    }

    [ContextMenu("Move")]
    IEnumerator CoMove()
    {
        pos = start;
        while (true)
        {
            path = aStar.Search(pos, end.pos);
            if(path.Count <= 1) break;
            transform.localPosition = new Vector3(path[1].x, 0, path[1].y);
            pos = path[1];
            yield return new WaitForSeconds(1);
        }
    }
}
