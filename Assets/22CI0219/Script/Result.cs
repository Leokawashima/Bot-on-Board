using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 勝敗表示用
/// </summary>
public class Result : MonoBehaviour
{
    private float timeCount = 3.0f; //表示する間隔

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
                Debug.Log("関数呼び出し");
                break;
            }
            else
            {
                yield return null;
            }
        }
    }


}
