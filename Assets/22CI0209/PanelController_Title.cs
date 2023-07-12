using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PanelController_Title : PanelController_SUPER
{
    [SerializeField] CanvasGroup panel_CG;
    [SerializeField] GameObject credits;
    [SerializeField] CanvasGroup credits_CG;
    bool isFading = false;

    

    /*以下、対応するButtonを押したときのメソッド*/
    async public void Button_ShowCredits()
    {
        /*多重呼び出し防止*/
        if(isFading){return;}
        isFading = true;

        /*各パネルを透明/不透明にする*/
        GlobalMember.DecreaseCGAlpha(panel_CG);
        await Task.Delay(500);
        ChangeEnabled();
        credits.SetActive(!credits.activeSelf);
        GlobalMember.IncreaseCGAlpha(credits_CG);
        await Task.Delay(500);
        isFading = false;
    }
    async public void Button_HideCredits()
    {
        /*多重呼び出し防止*/
        if(isFading){return;}
        isFading = true;

        /*各パネルを透明/不透明にする*/
        GlobalMember.DecreaseCGAlpha(credits_CG);
        await Task.Delay(500);
        credits.SetActive(!credits.activeSelf);
        ChangeEnabled();
        GlobalMember.IncreaseCGAlpha(panel_CG);
        await Task.Delay(500);
        isFading = false;
    }
}
