/*This script is written in UTF-8*/
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/*どこからでもアクセスできる関数*/
public class GlobalMember : MonoBehaviour
{
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
}
