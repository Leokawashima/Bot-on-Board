using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TweetScript : MonoBehaviour
{
    [SerializeField] string text = "TweetTest";
    [SerializeField] CreateQR QR;

    const string
        newText = "text=",
        newUrl = "&url=",
        newHashTag = "&hashtags=",
        newLine = "%0a",
        space = "%20",
        sharp = "%23";

    [ContextMenu("Tweet")]
    void Tweet()
    {
        var url = "https://twitter.com/intent/tweet?"
            + "text=" + text
            + newLine + newHashTag + "BotonBoard"
            + newLine + newHashTag + "BoB";

        QR.CreateQRCode(url);

#if UNITY_WEBGL
        Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
        //Application.OpenURL(url);
#endif
    }
}
