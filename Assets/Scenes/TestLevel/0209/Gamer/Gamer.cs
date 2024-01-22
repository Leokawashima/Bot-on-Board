using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参考：https://qiita.com/ga-mi-_qiita/items/ecba3343a14887bcb314
public class Gamer : MonoBehaviour
{
    [Header("ここに現在のマテリアルをドロップ")]
    Material material;
    [Header("虹色に光るスピード（0.001を推奨）")]
    [SerializeField]float gamingSpeed = 0.001f;
    float hue;

    void Start()
    {
        //スクリプトを付けたオブジェクトのみ光らせる
        material = GetComponent<Renderer>().material;
        hue = 0;
    }

    void Update()
    {
        hue += gamingSpeed;
        if (hue >= 1.0f) hue = 0;
        material.color = Color.HSVToRGB(hue, 1, 1);
    }
}
