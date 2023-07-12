/*This script is written in UTF-8*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*ゲーム画面に進むボタン*/
public class ButtonController_GoGame : MonoBehaviour
{
    public void Button_GoGame()
    {
        /*ゲームを始める*/
        Initiate.Fade("Game",Color.black,1.0f);
    }
}
