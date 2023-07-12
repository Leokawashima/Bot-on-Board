using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderKeywordFilter.FilterAttribute;

public class Order : MonoBehaviour
{
    protected enum StateOrder
    {
        Non, //まだ決まっていない
        Precedence, //先行
        Follower //後行
    }

    [SerializeField] Canvas OrderCanvas;
    [SerializeField] Button PrecedenceButton; //先行ボタン
    [SerializeField] Button FollowerButton; //後行ボタン
    [SerializeField] Button DecideButton; //決定ボタン
    [SerializeField] Image SelectImage; //選択したボタンが分かりやすいように作成した画像
    [SerializeField] Text InductionText; //プレイヤーがするべきことへ誘導するテキスト
    protected StateOrder order = StateOrder.Non; //先行、後行が決まるまで何も決まっていない状態にしておく

    [SerializeField] Text testText;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(order);

        PrecedenceButton.onClick.AddListener(Precedence_);
        FollowerButton.onClick.AddListener(Follower_);
        DecideButton.onClick.AddListener(Decide);

        SelectImage.enabled = false;
        testText.enabled = false;

        InductionText.text = "先行 後行を選んでください";
    }

    //PrecedenceButtonが押されたとき
    void Precedence_()
    {
        SelectImage.enabled = true;
        SelectImage.transform.position = PrecedenceButton.transform.position + new Vector3(200, 98, 0);
        order = StateOrder.Precedence;
        Debug.Log(order);

        InductionText.text = "先行でよろしいでしょうか";
    }

    //FollowerButtonが押されたとき
    void Follower_()
    {
        SelectImage.enabled = true;
        SelectImage.transform.position = FollowerButton.transform.position + new Vector3(200, 98, 0);
        order = StateOrder.Follower;
        Debug.Log(order);

        InductionText.text = "後行でよろしいでしょうか";
    }

    //DecideButtonが押されたとき
    void Decide()
    {
        //先行、後行が選択されていない場合、この先の処理は行わない
        if (order == StateOrder.Non) return;

        OrderCanvas.enabled = false;
        testText.enabled = true;
        testText.text = "あなたは" + order;
    }
}
