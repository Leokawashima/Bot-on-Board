using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 勝敗表示用
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0219　後藤
public class Result : MonoBehaviour
{
    private float timeCount = 1.5f; //表示する間隔

    [SerializeField] Text WinnerText; //勝者の名前を表示するテキスト
    [SerializeField] Text text; //「勝者は」

    private InputActionMapSettings inputActions;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ResultDisplay());
        inputActions = new InputActionMapSettings();
        inputActions.Enable();
    }

    //勝敗表示のコルーチン
    IEnumerator ResultDisplay()
    {
        text.text = "勝者は";
        yield return new WaitForSeconds(timeCount);
        WinnerText.text = "******";
        while(true)
        {
            if(inputActions.UI.Click.triggered)
            {
                Kansuu();
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    void Kansuu()
    {
        Debug.Log("関数呼び出し");
    }
}
