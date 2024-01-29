using System.Collections.Generic;
using UnityEngine;

public class InfoScrollViewManager : SingletonMonoBehaviour<InfoScrollViewManager>
{
    [SerializeField] private Transform m_content;
    [SerializeField] private InfoPlayerSetting m_prefab;

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

    private const string NAME_DAFAULT = "ナナシ";

    public void Initialize()
    {
        var _pmButton = GameSettingManager.Singleton.PlayerPMButton;
        Infos = new(_pmButton.Value);
        for (int i = 0; i < _pmButton.Value; ++i)
        {
            Infos.Add(CreateInfo(i, NAME_DAFAULT));
        }

        _pmButton.Event_ValueAdd += OnValueAdd;
        _pmButton.Event_ValueSub += OnValueSub;

        void OnValueAdd(int value_)
        {
            for (int i = 0; i < value_; ++i)
            {
                Infos.Add(CreateInfo(Infos.Count + i, NAME_DAFAULT));
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
        var _setting = new PlayerSetting();
        _setting.Initialize(index_, name_, colors[index_], 1, 4);
        _info.Initlaize(_setting);
        _info.Event_Click += PlayerSettingManager.Singleton.Select;
        return _info;
    }
}