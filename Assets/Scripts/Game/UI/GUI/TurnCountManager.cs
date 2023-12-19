using UnityEngine;
using TMPro;

//完成
public class TurnCountManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TurnText;

    public void SetTurn(int turn_)
    {
        m_TurnText.text = turn_.ToString();
    }
}