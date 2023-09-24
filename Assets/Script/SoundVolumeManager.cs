using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SoundVolumeManager : MonoBehaviour
{
    #region Fields
    [Header("AoudioMixier")]
    [SerializeField] AudioMixer m_AudioMixer;
    [Header("Sliders")]
    [SerializeField] Slider m_MasterSlider;
    [SerializeField] Slider m_BGMSlider;
    [SerializeField] Slider m_SESlider;
    [SerializeField] Slider m_UISlider;
    [Header("InputFields")]
    [SerializeField] TMP_InputField m_MastertInputField;
    [SerializeField] TMP_InputField m_BGMInputField;
    [SerializeField] TMP_InputField m_SEInputField;
    [SerializeField] TMP_InputField m_UIInputField;
    [Header("Toggle")]
    [SerializeField] Toggle m_MuteSoundToggle;

    readonly string m_FilePath = Name.Setting.SettingFilePath + "/sound.json";

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

    #region UnityFunctions

    void Start()
    {
        m_MuteSoundToggle.onValueChanged.AddListener((bool v) =>
        {
            AudioListener.volume = v ? 0 : 1;
        });

        void Sliderf(float v, string name, TMP_InputField inputfield)
        {
            m_AudioMixer.SetFloat(name, v);
            inputfield.text = (v + 80).ToString();
        }
        void InputFieldf(string str, string name, Slider slider)
        {
            if(str == string.Empty) str = "0";
            m_AudioMixer.SetFloat(name, int.Parse(str) - 80);
            slider.value = int.Parse(str) - 80;
        }

        {
            m_AudioMixer.GetFloat(Name.AudioMixer.Volume.Master, out float value);
            m_MasterSlider.value = value;
            m_MasterSlider.onValueChanged.AddListener((float v) => { Sliderf(v, Name.AudioMixer.Volume.Master, m_MastertInputField); });
            m_MastertInputField.text = (value + 80).ToString();
            m_MastertInputField.onSelect.AddListener((string str) => { m_MastertInputField.text = string.Empty; });
            m_MastertInputField.onEndEdit.AddListener((string str) => { InputFieldf(str, Name.AudioMixer.Volume.Master, m_MasterSlider); });
        }
        {
            m_AudioMixer.GetFloat(Name.AudioMixer.Volume.BGM, out float value);
            m_BGMSlider.value = value;
            m_BGMSlider.onValueChanged.AddListener((float v) => { Sliderf(v, Name.AudioMixer.Volume.BGM, m_BGMInputField); });
            m_BGMInputField.text = (value + 80).ToString();
            m_BGMInputField.onSelect.AddListener((string str) => { m_BGMInputField.text = string.Empty; });
            m_BGMInputField.onEndEdit.AddListener((string str) => { InputFieldf(str, Name.AudioMixer.Volume.BGM, m_BGMSlider); });
        }
        {
            m_AudioMixer.GetFloat(Name.AudioMixer.Volume.SE, out float value);
            m_SESlider.value = value;
            m_SESlider.onValueChanged.AddListener((float v) => { Sliderf(v, Name.AudioMixer.Volume.SE, m_SEInputField); });
            m_SEInputField.text = (value + 80).ToString();
            m_SEInputField.onSelect.AddListener((string str) => { m_SEInputField.text = string.Empty; });
            m_SEInputField.onEndEdit.AddListener((string str) => { InputFieldf(str, Name.AudioMixer.Volume.SE, m_SESlider); });
        }
        {
            m_AudioMixer.GetFloat(Name.AudioMixer.Volume.UI, out float value);
            m_UISlider.value = value;
            m_UISlider.onValueChanged.AddListener((float v) => { Sliderf(v, Name.AudioMixer.Volume.UI, m_UIInputField); });
            m_UIInputField.text = (value + 80).ToString();
            m_UIInputField.onSelect.AddListener((string str) => { m_UIInputField.text = string.Empty; });
            m_UIInputField.onEndEdit.AddListener((string str) => { InputFieldf(str, Name.AudioMixer.Volume.UI, m_UISlider); });
        }

        #region 悲しみのforナシ直書きの理由

        /*
        var names = new string[4] { Name.AudioMixer.Volume.Master, Name.AudioMixer.Volume.BGM, Name.AudioMixer.Volume.SE, Name.AudioMixer.Volume.UI };
        var sliders = new Slider[4] { m_MasterSlider, m_BGMSlider, m_SESlider, m_UISlider };
        var inputfields = new TMP_InputField[4] { m_MastertInputField, m_BGMInputField, m_SEInputField, m_UIInputField };

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
        本来であればこのようにforでまとめて書きたかったがコールバックを登録している都合上関数内部に配列からインデックスで取り出している変数があるのがよろしくなく実行するとエラーがでる
        関数配列で実装のほうが関数以外の場所は処理をまとめて実装できるがdelegateによる関数型宣言もろもろ含めてフィールド等へのコードが散見してしまうので泣きながらそれぞれ別に記述
         */

        #endregion 悲しみのforナシ直書きの理由
    }

    #endregion UnityFunctions

    #region Functions

    [ContextMenu("Save")]
    public void Save()
    {
        var save = new SaveData
        {
            MuteSound = m_MuteSoundToggle.isOn,
            MasterVolume = m_MasterSlider.value,
            BGMVolume = m_BGMSlider.value,
            SEVolume = m_SESlider.value,
            UIVolume = m_UISlider.value
        };

        string str = JsonUtility.ToJson(save);

        if(!Directory.Exists(Name.Setting.SettingFilePath))
        {
            Directory.CreateDirectory(Name.Setting.SettingFilePath);
        }

        StreamWriter sw = new StreamWriter(m_FilePath, false);
        sw.Write(str);
        sw.Flush();
        sw.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if(!Directory.Exists(Name.Setting.SettingFilePath))
        {
#if UNITY_EDITOR
            Debug.Log("セーブないぞ");
#endif
            return;
        }

        StreamReader sr = new StreamReader(m_FilePath);
        string str = sr.ReadToEnd();
        sr.Close();

        var save = JsonUtility.FromJson<SaveData>(str);

        m_MuteSoundToggle.isOn = save.MuteSound;
        m_MasterSlider.value = save.MasterVolume;
        m_BGMSlider.value = save.BGMVolume;
        m_SESlider.value = save.SEVolume;
        m_UISlider.value = save.UIVolume;
    }
    #endregion Functions
}