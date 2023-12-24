using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public static class Initiate
{
    static bool areWeFading = false;

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        areWeFading = false;
    }

    //Create Fader object and assing the fade scripts and assign all the variables
    public static void Fade(string before_, string after_, Color col, float multiplier)
    {
        if (areWeFading)
        {
            Debug.Log("Already Fading");
            return;
        }

        GameObject init = new GameObject();
        init.name = "Fader";
        Canvas myCanvas = init.AddComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myCanvas.sortingOrder = 10;
        init.AddComponent<CanvasGroup>();
        init.AddComponent<Image>();
        var scr = init.AddComponent<Fader>();

        scr.fadeDamp = multiplier;
        scr.fadeScene = before_;
        scr.fadeClear = after_;
        scr.fadeColor = col;
        scr.start = true;
        areWeFading = true;
        scr.InitiateFader();
        
    }

    public static void DoneFading() {
        areWeFading = false;
    }
}
