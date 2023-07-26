using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject room;

    public void OnClick_Start()
    {
        menu.SetActive(false);
        room.SetActive(true);
    }
}
