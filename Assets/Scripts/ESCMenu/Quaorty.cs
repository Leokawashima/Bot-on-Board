using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// まだ調整中　画面設定やクオリティを設定するクラス
/// </summary>
public class Quaorty : MonoBehaviour
{
    [SerializeField] TMP_Dropdown m_drop;
    [SerializeField] TMP_Dropdown m_sizedrop;
    [SerializeField] VolumeProfile m_volume;
    [SerializeField] Slider m_slider;

    void Start()
    {
        List<string> names = QualitySettings.names.ToList();
        m_drop.options.Clear();
        m_drop.AddOptions(names);
        m_drop.onValueChanged.AddListener((int v) => { QualitySettings.SetQualityLevel(v); });

        m_sizedrop.AddOptions(new List<string> { "FullScreen", "Window", "BoraderLess", "???", "Half", "Full" });
        m_sizedrop.onValueChanged.AddListener((int v) =>
        {
#if !UNITY_ANDROID
            switch(v)
            {
                case 0:
                    Screen.SetResolution(Screen.width, Screen.height, true);
                    break;
                case 1:
                    Screen.SetResolution(Screen.width, Screen.height, false);
                    break;
                case 2:
                    Kogane.ExeWindowFrameChanger.ChangeToBorderless();
                    break;
                case 3:
                    Kogane.ExeWindowFrameChanger.ChangeToDefault();
                    break;
                case 4:
                    Screen.SetResolution(1920, 1080, false);
                    break;
                case 5:
                    Screen.SetResolution(960, 540, false);
                    break;
            }
#endif
        });

        m_volume.TryGet(out ColorAdjustments light);
        m_slider.value = light.postExposure.value;
        m_slider.onValueChanged.AddListener((float v) =>
        {
            light.postExposure.value = v;
        });

#if !UNITY_EDITOR && UNITY_ANDROID
        sizedrop.interactable = false;
#endif
    }

    public void ScreenShot_Start()
    {
        if(!Directory.Exists(Name.FilePath.FilePath_ScreenShot))
        {
            Directory.CreateDirectory(Name.FilePath.FilePath_ScreenShot);
        }

        var now = DateTime.Now;
        string photoname = now.Year.ToString() + "." + now.Month.ToString() + "." + now.Day.ToString() + "_" + now.Hour.ToString() + "." + now.Minute.ToString() + "." + now.Second.ToString();
        ScreenCapture.CaptureScreenshot(Name.FilePath.FilePath_ScreenShot + $"/{photoname}.png");
    }
}