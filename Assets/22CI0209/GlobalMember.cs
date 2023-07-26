/*This script is written in UTF-8*/
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/*どこからでもアクセスできる関数*/
//　制作者　日本電子専門学校　ゲーム制作科　22CI0209　荻島
public class GlobalMember
{
    public static bool openingMenu { get; private set; } = false;

    /*フラグ等リセット*/
    public static void ResetProgress()
    {
        openingMenu =false;
    }

    /*ゲームプレイ終了*/
    public static void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /*パネルを透明にする(CanvasGroupのアタッチが必要)*/
    public static async void DecreaseCGAlpha(CanvasGroup cg)
    {
        for(int i = 0; i < 10; ++i)
        {
            cg.alpha -= 0.1f;
            await Task.Delay(50);
        }
    }
    /*パネルを不透明にする(CanvasGroupのアタッチが必要)*/
    public static async void IncreaseCGAlpha(CanvasGroup cg)
    {
        for(int i = 0; i < 10; ++i)
        {
            cg.alpha += 0.1f;
            await Task.Delay(50);
        }
    }

    /*一時停止を伴うメニュー開閉*/
    public static void OpenCloseMenu()
    {
        openingMenu = !openingMenu;
    }
}
