using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class ItemSelectManager : MonoBehaviour
{
    [SerializeField] ItemSelectInfo[] items;
    int[] create = new int[3] { 0, 1, 2 };

    public ItemSelectInfo select;

    RectTransform rect;
    [SerializeField] float start = -250;
    [SerializeField] float offset = 250;

    void Start()
    {
        rect = transform as RectTransform;
        for (int i = 0; i < create.Length; ++i)
        {
            var pos = rect.position + new Vector3(start +  i * offset, 0, 0);
            var go = Instantiate(items[create[i]], pos, Quaternion.identity, transform);
            go.GetComponent<ItemSelectInfo>().initialize(this, i);
        }
    }
}
