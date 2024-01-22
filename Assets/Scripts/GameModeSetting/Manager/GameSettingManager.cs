using UnityEngine;

public class GameSettingManager : MonoBehaviour
{
    // 現在の処理やフィールドはターンルールに依存した処理　いずれ分離する
    [SerializeField] private PlusMinusButton m_suddonDeathPlusMinusButton;
    [SerializeField] private PlusMinusButton m_forceFinishPlusMinusButton;
    public void Initialize()
    {

    }
}