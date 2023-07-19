/*This script is written in UTF-8*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*パネル制御：各シーンで、ESCキーを押した時に出現するパネル
  メニューを開いている間は一部の処理が一時停止するようにする*/
public class PanelController_ESC : PanelController_SUPER
{
    private InputMaster input;  //Input Action

    private void Awake()
    {
        input = new InputMaster();
        input.Menu.Quit.performed += PushEsc;
        input.Enable();
    }

    /// <summary>
    /// ボタン入力でメニューを開く
    /// </summary>
    /// <param name="context">入力</param>
    private void PushEsc(InputAction.CallbackContext context)
    {
        GlobalMember.OpenCloseMenu();
        ChangeEnabled();
    }

    /*以下、対応するButtonを押したときのメソッド*/
    public void Button_QuitYes()
    {
        /*ゲームプレイ終了*/
        GlobalMember.QuitGame();
    }
    public void Button_QuitNo()
    {
        GlobalMember.OpenCloseMenu();
        ChangeEnabled();
    }
}
