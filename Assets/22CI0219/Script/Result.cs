using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    private float timeCount = 3.0f; //表示する間隔
    [SerializeField] Text  WinnerText; //勝者を表示するテキスト
    [SerializeField] GameObject Induction; //↓のUI関連をまとめたもの
    [SerializeField] Text SelectText; //再接続するかタイトルに戻るかを表示するテキスト
    [SerializeField] Button AgainButton; //再接続ボタン
    [SerializeField] Button TitleButton; //タイトルに戻るボタン

    // Start is called before the first frame update
    void Start()
    {
        Induction.SetActive(false);

        AgainButton.onClick.AddListener(Again);
        TitleButton.onClick.AddListener(Title);

        StartCoroutine(ResultDisplay());
    }

    IEnumerator ResultDisplay()
    {
        WinnerText.text = "勝者は******";
        yield return new WaitForSeconds(timeCount);
        Induction.SetActive(true);
    }

    //AgainButtonが押されたとき
    void Again()
    {
        Debug.Log("再接続");
    }

    //TitleButtonが押されたとき
    void Title()
    {
        Debug.Log("タイトルに戻る");
    }
}
