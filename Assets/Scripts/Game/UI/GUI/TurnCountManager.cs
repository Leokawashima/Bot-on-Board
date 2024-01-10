using UnityEngine;
using TMPro;
using Game.GameRule;

public class TurnCountManager : MonoBehaviour
{
    [SerializeField] TMP_Text m_TurnText;

    private void OnEnable()
    {
        GameRule_Template.Event_TurnChanged += SetTurn;
    }
    private void OnDisable()
    {
        GameRule_Template.Event_TurnChanged -= SetTurn;
    }

    private void SetTurn(int turn_)
    {
        m_TurnText.text = turn_.ToString();
    }
}