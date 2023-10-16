using MyFileSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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

    readonly string m_FILEPATH = Name.Setting.FilePath_Setting + "/SoundSetting.json";

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

        /*
         *         {
            m_audioMixer.GetFloat(Name.AudioMixer.Volume.Master, out float _value);
            m_masterSlider.value = _value;
            m_masterSlider.onValueChanged.AddListener((float value_) => { Sliderf(value_, Name.AudioMixer.Volume.Master, m_mastertInputField); });
            m_mastertInputField.text = (_value + 80).ToString();
            m_mastertInputField.onSelect.AddListener((string str_) => { m_mastertInputField.text = string.Empty; });
            m_mastertInputField.onEndEdit.AddListener((string str_) => { InputFieldf(str_, Name.AudioMixer.Volume.Master, m_masterSlider); });
        }
        {
            m_audioMixer.GetFloat(Name.AudioMixer.Volume.BGM, out float _value);
            m_bgmSlider.value = _value;
            m_bgmSlider.onValueChanged.AddListener((float value_) => { Sliderf(value_, Name.AudioMixer.Volume.BGM, m_bgmInputField); });
            m_bgmInputField.text = (_value + 80).ToString();
            m_bgmInputField.onSelect.AddListener((string str_) => { m_bgmInputField.text = string.Empty; });
            m_bgmInputField.onEndEdit.AddListener((string str_) => { InputFieldf(str_, Name.AudioMixer.Volume.BGM, m_bgmSlider); });
        }
        {
            m_audioMixer.GetFloat(Name.AudioMixer.Volume.SE, out float _value);
            m_seSlider.value = _value;
            m_seSlider.onValueChanged.AddListener((float value_) => { Sliderf(value_, Name.AudioMixer.Volume.SE, m_seInputField); });
            m_seInputField.text = (_value + 80).ToString();
            m_seInputField.onSelect.AddListener((string str_) => { m_seInputField.text = string.Empty; });
            m_seInputField.onEndEdit.AddListener((string str_) => { InputFieldf(str_, Name.AudioMixer.Volume.SE, m_seSlider); });
        }
        {
            m_audioMixer.GetFloat(Name.AudioMixer.Volume.UI, out float _value);
            m_uiSlider.value = _value;
            m_uiSlider.onValueChanged.AddListener((float value_) => { Sliderf(value_, Name.AudioMixer.Volume.UI, m_uiInputField); });
            m_uiInputField.text = (_value + 80).ToString();
            m_uiInputField.onSelect.AddListener((string str_) => { m_uiInputField.text = string.Empty; });
            m_uiInputField.onEndEdit.AddListener((string str_) => { InputFieldf(str_, Name.AudioMixer.Volume.UI, m_uiSlider); });
        }
         */

        #region 悲しみのforナシ直書きの理由

        /*
        var names_ = new string[4] { Name.AudioMixer.Volume.Master, Name.AudioMixer.Volume.BGM, Name.AudioMixer.Volume.SE, Name.AudioMixer.Volume.UI };
        var sliders_ = new Slider[4] { m_masterSlider, m_bgmSlider, m_seSlider, m_uiSlider };
        var inputfields_ = new TMP_InputField[4] { m_MastertInputField, m_BGMInputField, m_SEInputField, m_UIInputField };

        for(int i = 0; i < names.Length; ++i)
        {
            m_AudioMixer.GetFloat(names[i], out float value);
            sliders[i].value = value;
            sliders[i].onValueChanged.AddListener((float v) =>
            {
                m_AudioMixer.SetFloat(names[i], v);
                inputfields[i].text = (v + 80).ToString();
            });
            inputfields[i].text = (value + 80).ToString();
            inputfields[i].onSelect.AddListener((string str) => { inputfields[i].text = string.Empty; });
            inputfields[i].onEndEdit.AddListener((string str) =>
            {
                if(str == string.Empty) str = "0";
                m_AudioMixer.SetFloat(names[i], int.Parse(str) - 80);
                sliders[i].value = int.Parse(str) - 80;
            });
        }
         * 本来であればこのようにforでまとめて書きたかったがコールバックを登録している都合上
         * 関数内部に配列からインデックスで取り出している変数があるのがよろしくないのか実行するとエラーがでる
         * 関数配列で実装のほうが関数以外の場所は処理をまとめて実装できるがフィールド等へのコードが散見してしまったり実質的な記述量はむしろ増えるので泣きながらそれぞれ別に記述
         */

        #endregion 悲しみのforナシ直書きの理由
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

        JsonFileSystem.Save(m_FILEPATH, save);
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