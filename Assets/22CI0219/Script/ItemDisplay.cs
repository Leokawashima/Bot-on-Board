using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アイテムをボタン内に表示するスクリプト
/// </summary>
public class ItemDisplay : MonoBehaviour
{
    Text ButtonText;
    // Start is called before the first frame update
    void Start()
    {
        ButtonText = gameObject.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ButtonText.text = "回復";
    }
}
