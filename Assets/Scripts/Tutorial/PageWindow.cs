using UnityEngine;

public class PageWindow : MonoBehaviour
{
    [field: SerializeField] public int Index { get; private set; } = 0;

    [SerializeField] private GameObject[] m_pages;
    public int Size => m_pages.Length;

    public void Initialize()
    {
        if (Size <= 1)
        {
            return;
        }

        m_pages[0].SetActive(true);
        for (int i = 1; i < m_pages.Length; ++i)
        {
            m_pages[i].SetActive(false);
        }
    }

    public void PageForward()
    {
        m_pages[Index].SetActive(false);
        Index = (Index + 1) % m_pages.Length;
        m_pages[Index].SetActive(true);
    }
    public void PageBackward()
    {
        m_pages[Index].SetActive(false);
        Index = Index - 1 < 0 ? 0 : Index - 1;
        m_pages[Index].SetActive(true);
    }
}