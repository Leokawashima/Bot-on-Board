using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アイテムをボタン内に表示するスクリプト
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0219　後藤
public class ItemDisplay : MonoBehaviour
{
    Text ButtonText;
    int number;
    // Start is called before the first frame update
    void Start()
    {
        ButtonText = gameObject.GetComponentInChildren<Text>();
        number = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if(number == 1)
        {
            ButtonText.text = "回復";
        }
        if(number == 2)
        {
            ButtonText.text = "剣";
        }
        if (number == 3)
        {
            ButtonText.text = "弓";
        }
    }
}
