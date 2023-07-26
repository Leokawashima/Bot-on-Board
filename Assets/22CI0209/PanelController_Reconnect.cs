/*This script is written in UTF-8*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//　制作者　日本電子専門学校　ゲーム制作科　22CI0209　荻島
public class PanelController_Reconnect : PanelController_SUPER
{
    /*以下、対応するButtonを押したときのメソッド*/
    public void Button_ReconnectYes()
    {
        /*再接続する(再戦)*/
        Initiate.Fade("Game",Color.black,1.0f);
    }
    public void Button_ReconnectNo()
    {
        /*再接続しない(タイトルへ)*/
        Initiate.Fade("Title",Color.black,1.0f);
    }
    public void Button_Quit()
    {
        /*ゲームをやめる*/
        GlobalMember.QuitGame();
    }
}
