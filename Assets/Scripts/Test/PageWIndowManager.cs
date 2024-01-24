using UnityEngine;
using UnityEngine.UI;

public class PageWIndowManager : MonoBehaviour
{
    [SerializeField] private Button m_closeButton;

    [SerializeField] private Button m_forwardButton;
    [SerializeField] private Button m_backwardButton;

    [SerializeField] private PageWindow[] m_windows;

#if UNITY_EDITOR
    [SerializeField]
#endif
    private PageWindow m_target;

    public void Initialize()
    {
        m_target = m_windows[0];
    }

    private void PageForward()
    {

    }
    private void PageBackward()
    {

    }
}