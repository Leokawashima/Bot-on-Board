using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFileSystem;

public class ReplayScript : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float SaveLength = 5.0f;
    [SerializeField] float SaveDistance = 0.01f;

    readonly string m_FilePath = Name.Setting.FilePath_Setting + "/Replay.json";

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

        JsonFileSystem.Save(m_FilePath, save);

        Debug.Log("Saved");
    }

    [ContextMenu("Load")]
    public void Load()
    {
        StartCoroutine(LoadReplay());
    }

    public IEnumerator LoadReplay()
    {
        if(!JsonFileSystem.Load<ReplayData>(m_FilePath, out var save))
        {
#if UNITY_EDITOR
            Debug.Log("セーブないぞ");
#endif
            yield break;
        }

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