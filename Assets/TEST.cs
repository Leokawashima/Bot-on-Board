using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        ChatGPTConnection gpt = new ChatGPTConnection("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");
        await gpt.RequestAsync("こんにちは？");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
