using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{

    enum TurnState
    {
        None,
        First_,
        Second_,
        AI_
    }
    [SerializeField] GameObject First;
    [SerializeField] GameObject Second;
    [SerializeField] GameObject AI;

    TurnState state = TurnState.None;

    private void Start()
    {
        First.SetActive(false);
        Second.SetActive(false);
        AI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            state = TurnState.First_;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            state = TurnState.Second_;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            state = TurnState.AI_;
        }

        switch (state)
        {
            case TurnState.None:
                First.SetActive(false);
                Second.SetActive(false);
                AI.SetActive(false);
                break;
            case TurnState.First_:
                StartCoroutine(AnimationStart(First));
                break;
            case TurnState.Second_:
                StartCoroutine(AnimationStart(Second));
                break;
            case TurnState.AI_:
                StartCoroutine(AnimationStart(AI));
                break;
            default:
                break;
        }
    }

    IEnumerator AnimationStart(GameObject gameObject)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        yield break;
    }
}
