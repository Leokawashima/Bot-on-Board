using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ReplayScript : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float SaveLength = 5.0f;
    [SerializeField] float SaveDistance = 0.01f;

    readonly string m_FilePath = Name.Setting.SettingFilePath + "/Replay.json";

    [ContextMenu("Save")]
    public void Save()
    {
        StartCoroutine(SaveReplay());
    }

    public IEnumerator SaveReplay()
    {
        Debug.Log("SaveStart");

        List<FrameData> frames = new List<FrameData>();
        for (int i = 0; i < SaveLength / SaveDistance; ++i)
        {
            var frame = new FrameData()
            {
                Pos = gameObject.transform.position,
                Rot = gameObject.transform.rotation,
            };
            frames.Add(frame);
            yield return new WaitForSeconds(SaveDistance);
        }

        var save = new ReplayData()
        {
            data = frames.ToArray(),
    };

        string str = JsonUtility.ToJson(save);

        if (!Directory.Exists(Name.Setting.SettingFilePath))
        {
            Directory.CreateDirectory(Name.Setting.SettingFilePath);
        }

        StreamWriter sw = new StreamWriter(m_FilePath, false);
        sw.Write(str);
        sw.Flush();
        sw.Close();

        Debug.Log("Saved");
    }

    [ContextMenu("Load")]
    public void Load()
    {
        StartCoroutine(LoadReplay());
    }

    public IEnumerator LoadReplay()
    {
        if (!Directory.Exists(Name.Setting.SettingFilePath))
        {
#if UNITY_EDITOR
            Debug.Log("セーブないぞ");
#endif
            yield break;
        }

        Debug.Log("LoadStart");

        StreamReader sr = new StreamReader(m_FilePath);
        string str = sr.ReadToEnd();
        sr.Close();

        var save = JsonUtility.FromJson<ReplayData>(str);

        rb.isKinematic = true;

        for (int i = 0; i < SaveLength / SaveDistance; ++i)
        {
            gameObject.transform.position = save.data[i].Pos;
            gameObject.transform.rotation = save.data[i].Rot;

            yield return new WaitForSeconds(SaveDistance);
        }

        rb.isKinematic = false;

        Debug.Log("Loaded");
    }

    [ContextMenu("AddPower")]
    public void AddPower()
    {
        rb.AddForce(new Vector3(0, UnityEngine.Random.Range(10, 15), 0), ForceMode.Impulse);
        rb.AddTorque(Vector3.right * UnityEngine.Random.Range(0, 100) + Vector3.up * UnityEngine.Random.Range(0, 100));
    }
}

[Serializable]
public struct ReplayData
{
    public FrameData[] data;
}

[Serializable]
public struct FrameData
{
    public Vector3 Pos;
    public Quaternion Rot;
}