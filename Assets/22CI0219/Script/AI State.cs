using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AIの状態を読み込みUIに反映させるスクリプト
/// /// </summary>
public class AIState : MonoBehaviour
{
    [SerializeField] Image Poison;
    [SerializeField] Image Stun;
    // Start is called before the first frame update
    void Start()
    {
        Poison.enabled = false;
        Stun.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Player_AI_Script.state == poison)
        //{
        //    Poison.enabled = true;
        //}
        //else
        //{
        //    Poison.enabled = false;
        //}
    }
}
