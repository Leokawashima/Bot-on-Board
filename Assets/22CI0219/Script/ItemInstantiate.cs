using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Canvasに適用してButtonを表示予定
/// </summary>
public class ItemInstantiate : MonoBehaviour
{
    [SerializeField] Button ItemButtonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(ItemButtonPrefab, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
