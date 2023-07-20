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
    [SerializeField] Image Die;

    enum AIstate
    {
        Non,
        poison,
        stun,
        die
    }
    AIstate state = AIstate.Non;
    // Start is called before the first frame update
    void Start()
    {
        Poison.enabled = false;
        Stun.enabled = false;
        Die.enabled = false;
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

        if (state == AIstate.poison)
        {
            Poison.enabled = true;
        }
        else
        {
            Poison.enabled = false;
        }
        if (state == AIstate.stun)
        {
            Stun.enabled = true;
        }
        else
        {
            Stun.enabled = false;
        }
        if (state == AIstate.die)
        {
            Die.enabled = true;
        }
        else
        {
            Die.enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.A)) {
            state = AIstate.stun;
        }

        if (Input.GetKeyDown(KeyCode.B)){
            state = AIstate.poison;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            state = AIstate.die;
        }
    }
}
