using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using MyFileSystem;

public class SoundVolumeManager : MonoBehaviour
{
    #region Fields
    [Header("AoudioMixier")]
    [SerializeField] AudioMixer m_audioMixer;
    [Header("Sliders")]
    [SerializeField] Slider m_masterSlider;
    [SerializeField] Slider m_bgmSlider;
    [SerializeField] Slider m_seSlider;
    [SerializeField] Slider m_uiSlider;
    [Header("InputFields")]
    [SerializeField] TMP_InputField m_mastertInputField;
    [SerializeField] TMP_InputField m_bgmInputField;
    [SerializeField] TMP_InputField m_seInputField;
    [SerializeField] TMP_InputField m_uiInputField;
    [Header("Toggle")]
    [SerializeField] Toggle m_muteSoundToggle;

    readonly string m_FILEPATH = Name.FilePath.FilePath_Setting + "/SoundSetting.json";

    // 値が変更されているかどうかのフラグ
    // 変更したらフラグを立ててフラグの状態をプロパティで確認されたら自動的にフラグを折る
    bool m_isDirty = false;
    public bool IsDirty { get {
            var flag = m_isDirty;
            m_isDirty = false;
            return flag;
        }
    }

    struct SaveData
    {
        public bool
            MuteSound;
        public float
            MasterVolume,
            BGMVolume,
            SEVolume,
            UIVolume;
    }

    #endregion Fields

    #region Functions

    public void Initialize()
    {
        m_muteSoundToggle.onValueChanged.AddListener((bool v) =>
        {
            AudioListener.volume = v ? 0 : 1;
            m_isDirty |= true;
        });

        void Sliderf(float v, string name, TMP_InputField inputfield)
        {
            m_audioMixer.SetFloat(name, v);
            inputfield.text = (v + 80).ToString();
            m_isDirty |= true;
        }
        void InputFieldf(string str, string name, Slider slider)
        {
            if(str == string.Empty) str = "0";
            m_audioMixer.SetFloat(name, int.Parse(str) - 80);
            slider.value = int.Parse(str) - 80;
            m_isDirty |= true;
        }

        void _Initialize(string name_, Slider slider_, TMP_InputField inputField_)
        {
            m_audioMixer.GetFloat(name_, out float _value);
            slider_.value = _value;
            slider_.onValueChanged.AddListener((float value_) => { Sliderf(value_, name_, inputField_); });
            inputField_.text = (_value + 80).ToString();
            inputField_.onSelect.AddListener((string str_) => { inputField_.text = string.Empty; });
            inputField_.onEndEdit.AddListener((string str_) => { InputFieldf(str_, name_, slider_); });
        }

        _Initialize(Name.AudioMixer.Volume.Master, m_masterSlider, m_mastertInputField);
        _Initialize(Name.AudioMixer.Volume.BGM, m_bgmSlider, m_bgmInputField);
        _Initialize(Name.AudioMixer.Volume.SE, m_seSlider, m_seInputField);
        _Initialize(Name.AudioMixer.Volume.UI, m_uiSlider, m_uiInputField);
    }

    [ContextMenu("Save")]
    public void Save()
    {
        var save = new SaveData
        {
            MuteSound = m_muteSoundToggle.isOn,
            MasterVolume = m_masterSlider.value,
            BGMVolume = m_bgmSlider.value,
            SEVolume = m_seSlider.value,
            UIVolume = m_uiSlider.value
        };

        JsonFileSystem.SaveToJson(m_FILEPATH, save);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (false == JsonFileSystem.Load<SaveData>(m_FILEPATH, out var save))
        {
            return;
        }

        m_muteSoundToggle.isOn = save.MuteSound;
        m_masterSlider.value = save.MasterVolume;
        m_bgmSlider.value = save.BGMVolume;
        m_seSlider.value = save.SEVolume;
        m_uiSlider.value = save.UIVolume;
    }
    #endregion Functions
}