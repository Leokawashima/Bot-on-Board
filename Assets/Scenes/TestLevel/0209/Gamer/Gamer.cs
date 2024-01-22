using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamer : MonoBehaviour
{
    [Header("ここに現在のマテリアルをドロップ")]
    [SerializeField]Material material;
    [Header("虹色に光るスピード（0.001を推奨）")]
    [SerializeField]float gamingSpeed = 0.001f;
    float hue;

    // Start is called before the first frame update
    void Start()
    {
        hue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        hue += gamingSpeed;
        if (hue >= 1.0f) hue = 0;
        material.color = Color.HSVToRGB(hue, 1, 1);
    }
}
