/*This script is written in UTF-8*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*パネルUI管理クラス(スーパー)
  ◎注意事項
  このスクリプトを継承したスクリプト(パネル毎)を使用してください
  そして、それを空のオブジェクトにアタッチした後、パネルをスクリプトにアタッチしてください
  これ単体ではオブジェクトにアタッチしないでください*/
//　制作者　日本電子専門学校　ゲーム制作科　22CI0209　荻島
public class PanelController_SUPER : MonoBehaviour
{
    [SerializeField] GameObject panel;    

    /*アクティブ/非アクティブを切り替える*/
    protected void ChangeEnabled()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
