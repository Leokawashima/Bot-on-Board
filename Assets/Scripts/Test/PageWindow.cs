using UnityEngine;
using UnityEngine.UI;

public class PageWindow : MonoBehaviour
{
    [field: SerializeField] public Canvas[] Pages { get; private set; }

    [field: SerializeField] public int m_index { get; private set; } = 0;

    public void Initialize()
    {
        if (Pages.Length <= 1)
        {
            return;
        }

        for (int i = 1; i < Pages.Length; ++i)
        {
            Pages[i].enabled = false;
        }
    }

    public void PageForward()
    {
        
    }
    public void PageBackward()
    {

    }
}