using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Button TitleButton;

    void Start()
    {
        TitleButton.onClick.AddListener(OnButton_Title);
    }

    void OnButton_Title()
    {
        Initiate.Fade(Name.Scene.Title, Color.black, 1.0f);
    }
}
