using System.Collections.Generic;
using UnityEngine;

public class InfoPlayerSettingManager : MonoBehaviour
{
    [SerializeField] private PlusMinusButton m_plusMinusButton;
    [SerializeField] private PlusMinusButton m_botPMButton;
    [SerializeField] private Transform m_content;
    [SerializeField] private InfoPlayerSetting m_prefab;

    [SerializeField] private ShowInfoPlayerSetting m_showInfoPlayerSetting;
#if UNITY_EDITOR
    [field: SerializeField]
#endif
    public List<InfoPlayerSetting> Infos { get; private set; }

    private readonly float[] colors = new float[]
    {
        0.0f,
        240.0f / 360.0f,
        120.0f / 360.0f,
        60.0f / 360.0f,
    };

    public void Initialize()
    {
        var _count = m_plusMinusButton.Value;
        Infos = new(_count);
        for (int i = 0; i < _count; ++i)
        {
            Infos.Add(CreateInfo(i, "Default"));
        }

        m_plusMinusButton.Event_ValueAdd += OnValueAdd;
        m_plusMinusButton.Event_ValueSub += OnValueSub;

        void OnValueAdd(int value_)
        {
            for (int i = 0; i < value_; ++i)
            {
                Infos.Add(CreateInfo(Infos.Count + i, "Default"));
            }
        }
        void OnValueSub(int value_)
        {
            for (int i = 0; i < value_; ++i)
            {
                var _info = Infos[Infos.Count - 1];
                Infos.Remove(_info);
                Destroy(_info.gameObject);
            }
        }
    }
    private InfoPlayerSetting CreateInfo(int index_, string name_)
    {
        var _info = Instantiate(m_prefab, m_content);
        var _setting = _info.gameObject.AddComponent<PlayerSetting>();
        _setting.Initialize(index_, name_, colors[index_], m_botPMButton);
        _info.Initlaize(_setting);
        _info.Event_Click += OnClick;
        return _info;
    }
    private void OnClick(InfoPlayerSetting info_)
    {
        m_showInfoPlayerSetting.Select(info_);
    }
}