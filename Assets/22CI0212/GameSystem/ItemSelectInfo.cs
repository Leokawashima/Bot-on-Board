using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class ItemSelectInfo : MonoBehaviour
{
    ItemSelectManager manager;
    int index;

    [SerializeField] string itemName;
    public string getItemName {  get { return itemName; } }

    public void initialize(ItemSelectManager manager_, int index_)
    {
        manager = manager_;
        index = index_;
    }
    public void OnClick_ItemInfo()
    {
        manager.select = this;
    }
}
