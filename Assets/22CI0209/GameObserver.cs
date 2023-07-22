using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

//　制作者　日本電子専門学校　ゲーム制作科　22CI0209　荻島
public class GameObserver : MonoBehaviour
{
     private void Awake() 
    {
        GlobalMember.ResetProgress();
    }
}