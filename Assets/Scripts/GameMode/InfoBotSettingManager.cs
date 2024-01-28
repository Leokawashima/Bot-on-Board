using System.Collections.Generic;
using UnityEngine;

public class InfoBotSettingManager : MonoBehaviour
{
    [SerializeField] InfoBotSetting m_prefab;
#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<InfoBotSetting> m_settings;

    public void Initialize(int max_, List<BotSetting> settings_)
    {
        m_settings = new(max_);
        for (int i = 0; i < settings_.Count; ++i)
        {
            Add(settings_[i]);
        }
    }

    public void Add(BotSetting data_)
    {
        var _info = Instantiate(m_prefab, transform);
        _info.Initialize(data_);
        m_settings.Add(_info);
    }
    public void Remove()
    {
        m_settings.RemoveAt(m_settings.Count - 1);
    }
}