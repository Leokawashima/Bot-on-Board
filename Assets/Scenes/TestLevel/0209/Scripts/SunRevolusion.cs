using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//公転
public class SunRevolution : MonoBehaviour
{
    [SerializeField] Vector3 revolutionPerSec;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(revolutionPerSec * Time.deltaTime);
    }
}
