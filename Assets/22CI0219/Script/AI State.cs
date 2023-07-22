using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AIの状態を読み込みUIに反映させるスクリプト
/// /// </summary>
public class AIState : MonoBehaviour
{
    [SerializeField] Image Normal;
    [SerializeField] Image Die;

    public Player_AI_Script playerAI;

    // Start is called before the first frame update
    void Start()
    {
        Normal.enabled = false;
        Die.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerAI.State == Player_AI_Script.AI_state.dead)
        //{
        //    Die.enabled = true;
        //}
        //else
        //{
        //    Normal.enabled = true;
        //}

        switch (playerAI.State)
        {
            case Player_AI_Script.AI_state.dead:
                Die.enabled = true;
                break;
            default:
                Normal.enabled = true;
                    break;
        }

    }
}
