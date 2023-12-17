using UnityEngine;

public class TitlePressAnyKey : MonoBehaviour
{
    [SerializeField] FlashSystem m_flashSystem;
    public FlashSystem FlashSystem => m_flashSystem;
    [SerializeField] RainbowText m_rainbowText;
    [SerializeField] AudioSource m_audio;
    [SerializeField] ParticleSystem m_particle;

    public void Initialize()
    {
        m_rainbowText.Active();
    }

    public void Enable()
    {
        m_rainbowText.gameObject.SetActive(true);
        m_particle.gameObject.SetActive(true);
    }

    public void Disable()
    {
        m_rainbowText.gameObject.SetActive(false);
        m_particle.gameObject.SetActive(false);
    }

    public void Enter()
    {
        m_flashSystem.Active();
        m_audio.Play();
        m_particle.Play();
    }
}
