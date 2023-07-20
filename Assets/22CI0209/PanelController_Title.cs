using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PanelController_Title : PanelController_SUPER
{
    [SerializeField] CanvasGroup panel_CG;      //タイトル側canvas
    [SerializeField] TextMeshProUGUI text_PAK;      //PressAnyKey
    [SerializeField] GameObject button_Start;
    [SerializeField] GameObject button_Quit;
    [SerializeField] GameObject button_Credits;
    [SerializeField] GameObject credits;        //クレジット側パネル
    [SerializeField] CanvasGroup credits_CG;    //クレジット側canvas
    private InputMaster input;                  //Input Action
    bool pressedAnyKey = false;                 //何かボタンを押した
    bool isFading = false;                      //フェード中

    private void Awake()
    {
        input = new InputMaster();
        input.Menu.Anything.performed += PressAnyKey;
        input.Enable();
    }

    /*"ボタンを押してください"に従うとテキストが消滅しボタンが出現*/
    private void PressAnyKey(InputAction.CallbackContext context) 
    {
        if(pressedAnyKey){return;}
        pressedAnyKey = true;
        text_PAK.enabled = false;
        button_Start.SetActive(true);
        button_Quit.SetActive(true);
        button_Credits.SetActive(true);
    }

    /*以下、対応するButtonを押したときのメソッド*/
    public void Button_GoGame()
    {
        /*ゲームを始める*/
        Initiate.Fade("Game",Color.black,1.0f);
    }
    public void Button_QuitGame()
    {
        GlobalMember.QuitGame();
    }
    async public void Button_ShowCredits()
    {
        /*ESC中は動かない*/
        if(GlobalMember.openingMenu){return;}

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
        /*ESC中は動かない*/
        if(GlobalMember.openingMenu){return;}

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
