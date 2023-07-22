using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class ClickObjCatch : MonoBehaviour
{
    Vector2 mPos;
    void OnEnable()
    {
        var map = new InputActionMapSettings();
        var action = map.UI;
        map.Player.Fire.started += OnStarted;
        action.Point.started += OnMousePosition;
        action.Point.performed += OnMousePosition;
        action.Point.canceled += OnMousePosition;
        map.Enable();
    }
    void OnDisable()
    {
        var map = new InputActionMapSettings();
        var action = map.UI;
        map.Player.Fire.started -= OnStarted;
        action.Point.started -= OnMousePosition;
        action.Point.performed -= OnMousePosition;
        action.Point.canceled -= OnMousePosition;
        map.Disable();
    }
    void OnStarted(InputAction.CallbackContext context)
    {
        //画面内でクリックした場所にあるgameObjectを取得できるrayのサンプルコード

        //まずクリックした場所の座標をからレイを作る
        Ray ray = Camera.main.ScreenPointToRay(mPos);
        //作ったレイを使って実際に判定を行う
        if (Physics.Raycast(ray, out var hit))
        {
            //out という修飾子があるがようは値を返してくれるものを入れるところ
            //当たったらtrue　当たってないならfalse　当たった場合情報が hit に入っている
            //hit.gameObject　のようにいきなりgameObjectを取得はできない
            //hit.collider.gameObjectで当たったものを取得できる　あとは煮るなり焼くなり

            //以下はCubeをクリックしたことが分かりやすくするためのサンプルコード
            var mat = hit.collider.gameObject.GetComponent<Renderer>().material;
            float r = Random.Range(0.0f, 1.0f), g = Random.Range(0.0f, 1.0f), b = Random.Range(0.0f, 1.0f);
            mat.color = new Color(r, g, b, 1);
        }
    }
    void OnMousePosition(InputAction.CallbackContext context)
    {
        mPos = context.ReadValue<Vector2>();
    }

    void Start()
    {
        StartCoroutine(OneSecondColorChange());
    }
    IEnumerator OneSecondColorChange()
    {
        while(true)
        {
            //EventSystem.currentで現在アクティブなEventSystemを取得できる
            //currentSelectedGameObjectで現在選択されているゲームオブジェクト(UIのボタンやInputFieldなど)を取得できる
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                try
                {
                    //以下は選択されていることが分かりやすくするための色変え処理
                    var img = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
                    float r = Random.Range(0.0f, 1.0f), g = Random.Range(0.0f, 1.0f), b = Random.Range(0.0f, 1.0f);
                    img.color = new Color(r, g, b, 1);
                }
                catch
                {
                    Debug.Log("GetComponent Missing");
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
