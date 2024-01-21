using System.Collections.Generic;
using UnityEngine;

public class InfoPlayerSettingManager : MonoBehaviour
{
    [SerializeField] private PlusMinusButton m_plusMinusButton;
    [SerializeField] private Transform m_content;
    [SerializeField] private InfoPlayerSetting m_prefab;

#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<InfoPlayerSetting> m_infos;

    private readonly float[] colors = new float[]
    {
        0.0f,
        240.0f / 360.0f,
        120.0f / 360.0f,
        60.0f / 360.0f,
    };

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        var _count = m_plusMinusButton.Value;
        m_infos = new(_count);
        for (int i = 0; i < _count; ++i)
        {
            var _info = Instantiate(m_prefab, m_content);
            m_infos.Add(_info);
            _info.Set(i + 1, colors[i], "Default");
        }

        m_plusMinusButton.Event_ValueChanged += OnValueChanged;

        void OnValueChanged(int value_)
        {
            var _diff = m_infos.Count - value_;
            if (_diff < 0)
            {
                for (int i = 0, cnt = Mathf.Abs(_diff); i < cnt; ++i)
                {
                    var _info = Instantiate(m_prefab, m_content);
                    m_infos.Add(_info);
                    _info.Set(m_infos.Count + i, colors[m_infos.Count - 1 + i], "Default");
                }
            }
            else
            {
                for (int i = 0, cnt = Mathf.Abs(_diff); i < cnt; ++i)
                {
                    var _info = m_infos[m_infos.Count - 1];
                    m_infos.Remove(_info);
                    Destroy(_info.gameObject);
                }
            }
        }
    }
}