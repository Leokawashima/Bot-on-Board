using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleCredit : MonoBehaviour
{
    [SerializeField] GameObject m_credit;
    [SerializeField] Button m_backButton;
    [SerializeField] AudioSource m_audio;

    public event Action OnHideCredit;

    public void Initialize()
    {
        m_backButton.onClick.AddListener(OnClickBack);
    }

    void OnClickBack()
    {
        m_audio.Play();
        OnHideCredit?.Invoke();
    }

    public void Enable()
    {
        m_credit.SetActive(true);
    }
    public void Disable()
    {
        m_credit.SetActive(false);
    }
}