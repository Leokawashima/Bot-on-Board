using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreate : MonoBehaviour
{
    [SerializeField] MapObject_SO_Template so;
    void Start()
    {
        so.ObjectSpawn(new Vector2Int(10, 10));
    }
}