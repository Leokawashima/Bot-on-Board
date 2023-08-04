using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] Slider m_MainSlider;
    [SerializeField] Slider m_BGMSlider;
    [SerializeField] AudioMixer m_AudioMixer;
    [SerializeField] Button m_SaveButton;
    [SerializeField] Button m_LoadButton;

    [SerializeField] TMPro.TextMeshProUGUI m_TextMeshPro;

    void Start()
    {
        m_AudioMixer.GetFloat(Name.AudioMixer.Volume.Master, out float mainvalue);
        m_MainSlider.value = mainvalue;
        m_MainSlider.onValueChanged.AddListener((float v) => { m_AudioMixer.SetFloat(Name.AudioMixer.Volume.Master, v); });

        m_AudioMixer.GetFloat(Name.AudioMixer.Volume.BGM, out float bgmvalue);
        m_MainSlider.value = bgmvalue;
        m_BGMSlider.onValueChanged.AddListener((float v) => { m_AudioMixer.SetFloat(Name.AudioMixer.Volume.BGM, v); });

        m_SaveButton.onClick.AddListener(Save);
        m_LoadButton.onClick.AddListener(Load);
    }

    [ContextMenu("Save")]
    void Save()
    {
        var save = new SaveData
        {
            MainVolume = m_MainSlider.value,
            BGMVolume = m_BGMSlider.value,
        };

        string str = JsonUtility.ToJson(save);

        if(!Directory.Exists(Name.Setting.SettingFilePath))
        {
            Directory.CreateDirectory(Name.Setting.SettingFilePath);
        }

        StreamWriter sw = new StreamWriter(Name.Setting.SettingFilePath + "/sound.json", false);
        sw.Write(str);
        sw.Flush();
        sw.Close();

        m_TextMeshPro.text = "Save : " + Name.Setting.SettingFilePath + "/sound.json";
    }

    [ContextMenu("Load")]
    void Load()
    {
        if(!Directory.Exists(Name.Setting.SettingFilePath))
        {
            Debug.Log("セーブねえよ");
        }

        StreamReader sr = new StreamReader(Name.Setting.SettingFilePath + "/sound.json");
        string str = sr.ReadToEnd();
        sr.Close();

        var save = JsonUtility.FromJson<SaveData>(str);
        m_MainSlider.value = save.MainVolume;
        m_BGMSlider.value = save.BGMVolume;

        m_TextMeshPro.text = "Load : " + Name.Setting.SettingFilePath + "/sound.json";
    }
}

struct SaveData
{
    public float
        MainVolume,
        BGMVolume;
}
