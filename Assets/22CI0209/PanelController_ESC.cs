/*This script is written in UTF-8*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*パネル制御：各シーンで、ESCキーを押した時に出現するパネル*/
public class PanelController_ESC : PanelController_SUPER
{
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeEnabled();
        }
    }

    /*以下、対応するButtonを押したときのメソッド*/
    public void Button_QuitYes()
    {
        /*ゲームプレイ終了*/
        GlobalMember.QuitGame();
    }
    public void Button_QuitNo()
    {
        ChangeEnabled();
    }
}
