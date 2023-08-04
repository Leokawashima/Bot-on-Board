using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;

public class Quaorty : MonoBehaviour
{
    [SerializeField] TMP_Dropdown drop;

    void Start()
    {
        List<string> names = QualitySettings.names.ToList();
        drop.options.Clear();
        drop.AddOptions(names);
        drop.onValueChanged.AddListener((int v) => { QualitySettings.SetQualityLevel(v, true); });
    }
}
