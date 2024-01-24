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
    [SerializeField] bool reverse = false; //逆再生

    void Start()
    {
        //スクリプトを付けたオブジェクトのみ光らせる
        material = GetComponent<Renderer>().material;
        if(reverse) hue = 1;
        else hue = 0;
    }

    void Update()
    {
        if(reverse)
        {
            hue -= gamingSpeed;
            if (hue <= 0.0f) hue += 1.0f;
        }
        else
        {
            hue += gamingSpeed;
            if (hue >= 1.0f) hue -= 1.0f;
        }
        material.color = Color.HSVToRGB(hue, 1, 1);
    }
}
