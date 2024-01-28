using UnityEngine;

public class GameSettingManager : SingletonMonoBehaviour<GameSettingManager>
{
    [SerializeField] private Canvas m_canvas;

    [field: SerializeField] public PlusMinusButton PlayerPMButton { get; private set; }
    // 現在の処理やフィールドはターンルールに依存した処理　いずれ分離する
    [SerializeField] private PlusMinusButton m_suddonDeathPMButton;
    [SerializeField] private PlusMinusButton m_forceFinishPMButton;

    public void Enable() => m_canvas.enabled = true;
    public void Disable() => m_canvas.enabled = false;

    public void Initialize()
    {
        
    }
}