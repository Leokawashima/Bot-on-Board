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
/// まだ調整中
/// </summary>
public class Quaorty : MonoBehaviour
{
    [SerializeField] TMP_Dropdown drop;
    [SerializeField] TMP_Dropdown sizedrop;
    [SerializeField] VolumeProfile volume;
    [SerializeField] Slider slider;

    void Start()
    {
        List<string> names = QualitySettings.names.ToList();
        drop.options.Clear();
        drop.AddOptions(names);
        drop.onValueChanged.AddListener((int v) => { QualitySettings.SetQualityLevel(v); });

        sizedrop.AddOptions(new List<string> { "FullScreen", "Window", "BoraderLess", "???", "Half", "Full" });
        sizedrop.onValueChanged.AddListener((int v) =>
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

        volume.TryGet(out ColorAdjustments light);
        slider.value = light.postExposure.value;
        slider.onValueChanged.AddListener((float v) =>
        {
            light.postExposure.value = v;
        });

#if !UNITY_EDITOR && UNITY_ANDROID
        sizedrop.interactable = false;
#endif
    }

    public void ScreenShot_Start()
    {
        if(!Directory.Exists(Name.Setting.FilePath_ScreenShot))
        {
            Directory.CreateDirectory(Name.Setting.FilePath_ScreenShot);
        }

        var now = DateTime.Now;
        string photoname = now.Year.ToString() + "." + now.Month.ToString() + "." + now.Day.ToString() + "_" + now.Hour.ToString() + "." + now.Minute.ToString() + "." + now.Second.ToString();
        ScreenCapture.CaptureScreenshot(Name.Setting.FilePath_ScreenShot + $"/{photoname}.png");
    }
}