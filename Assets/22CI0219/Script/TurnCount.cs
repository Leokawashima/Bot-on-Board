using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ターン数をカウントして表示する用
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0219　後藤
public class TurnCount : MonoBehaviour
{
    [SerializeField] Text turnCountText;
    int turn; //現在のターン数

    void Start()
    {
        turn = 1;
    }
    // Update is called once per frame
    void Update()
    {
        turnCountText.text = turn + "ターン目";

        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddTurn();
        }
    }

    //ターン数を増やしたいタイミングでこの関数を呼び出せばOKなはず... 
    public void AddTurn()
    {
        turn++;
    }
}
