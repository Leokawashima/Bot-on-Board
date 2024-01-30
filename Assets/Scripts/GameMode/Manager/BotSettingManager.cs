using System.Collections.Generic;
using UnityEngine;

public class BotSettingManager : SingletonMonoBehaviour<BotSettingManager>
{
    [SerializeField] private Canvas m_canvas;

    [SerializeField] private PlusMinusButton m_hpPMButton;
    [SerializeField] private PlusMinusButton m_hpMaxPMButton;
    [SerializeField] private PlusMinusButton m_attackPMButton;

    public void Enable() => m_canvas.enabled = true;
    public void Disable() => m_canvas.enabled = false;

    public void Initialize()
    {

    }

    public void Select(List<BotSetting> settings_)
    {
        for (int i = 0; i < settings_.Count; ++i)
        {
            m_hpPMButton.SetValue((int)settings_[i].HP);
            m_hpMaxPMButton.SetValue((int)settings_[i].HPMax);
            m_attackPMButton.SetValue((int)settings_[i].Attack);
        }
    }
}