using System.Collections;
using UnityEngine;

public class PingTest : MonoBehaviour
{
    [ContextMenu("AAA")]
    private void Start()
    {
        StartCoroutine(a());

        static IEnumerator a()
        {
            var ping = new Ping("192.168.2.164");
            while (false == ping.isDone)
            {
                yield return null;
            }
            Debug.Log(ping.time);
        }
    }
}