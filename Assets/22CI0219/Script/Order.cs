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

    [SerializeField] Button PrecedenceButton; //先行ボタン
    [SerializeField] Button FollowerButton; //後行ボタン
    [SerializeField] Button DecideButton; //決定ボタン
    [SerializeField] Image SelectImage; //選択したボタンが分かりやすいように作成した画像
    protected StateOrder order = StateOrder.Non; //先行、後行が決まるまで何も決まっていない状態にしておく


    // Start is called before the first frame update
    void Start()
    {
        PrecedenceButton.onClick.AddListener(Precedence_);
        FollowerButton.onClick.AddListener(Follower_);
        DecideButton.onClick.AddListener(Decide);
        SelectImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Precedence_()
    {
        SelectImage.enabled = true;
        SelectImage.transform.position = PrecedenceButton.transform.position + new Vector3(200, 98, 0);
        order = StateOrder.Precedence;
    }

    void Follower_()
    {
        SelectImage.enabled = true;
        SelectImage.transform.position = FollowerButton.transform.position + new Vector3(200, 98, 0);
        order = StateOrder.Follower;
    }

    void Decide()
    {
        gameObject.SetActive(false);
    }
}
