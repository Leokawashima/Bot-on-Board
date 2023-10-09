using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class T_MenuScript : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject m_MenuDefault;
    [Header("Button-Default")]
    [SerializeField] Button m_StartButton;
    [SerializeField] Button m_QuitButton;
    [SerializeField] Button m_OptionButton;
    [SerializeField] Button m_CreditsButton;
    [Header("Sound")]
    [SerializeField] AudioSource m_Audio;
    [SerializeField] Animator m_Animator;
    [Header("Profile")]
    [SerializeField] Volume m_Volume;
    [Header("EscMenu")]
    [SerializeField] EscMenuManager m_EscMenu;

    public event Action OnMenuCredits;

    void OnEnable()
    {
        EscMenuManager.Event_EscMenuOpen += Disable;
        EscMenuManager.Event_EscMenuClose += Enable;
    }

    void OnDisable()
    {
        EscMenuManager.Event_EscMenuOpen -= Disable;
        EscMenuManager.Event_EscMenuClose -= Enable;
    }

    void Start()
    {
        m_StartButton.onClick.AddListener(OnButton_Play);
        m_QuitButton.onClick.AddListener(OnButton_Quit);
        m_OptionButton.onClick.AddListener(OnButton_Option);
        m_CreditsButton.onClick.AddListener(OnButton_Credits);
    }

    public void Enable()
    {
        m_MenuDefault.SetActive(true);
    }

    public void Disable()
    {
        m_MenuDefault.SetActive(false);
    }

    #region Menu-Default

    void OnButton_Play()
    {
        m_Audio.Play();
        m_MenuDefault.SetActive(false);

        IEnumerator Co()
        {
            m_Volume.enabled = true;

            m_Volume.profile.TryGet(out LensDistortion _lens);
            m_Volume.profile.TryGet(out Vignette _vignette);
            _lens.intensity.value = 0.3f;
            _lens.scale.value = 1.0f;
            _vignette.intensity.value = 0;
            IEnumerator Trasition(float time_, Action<float> update_, Action completed_)
            {
                float _erapsedTime = 0;
                while(true)
                {
                    _erapsedTime += Time.deltaTime;
                    if(_erapsedTime > time_)
                    {
                        completed_();
                        yield break;
                    }
                    update_(_erapsedTime);
                    yield return null;
                }
            }

            yield return Trasition(1.0f,
                (float time_) =>
                {
                    _lens.intensity.value = 0.3f + time_ * 0.4f;
                    _vignette.intensity.value = time_ * 0.5f;
                },
                () =>
                {
                    _lens.intensity.value = 0.7f;
                    _vignette.intensity.value = 0.5f;
                });

            yield return Trasition(0.05f,
                (float time_) =>
                {
                    _lens.intensity.value = 0.7f - time_ * 1.7f / 0.05f;
                },
                () =>
                {
                    _lens.intensity.value = -1.0f;
                });

            yield return Trasition(0.15f,
                (float time_) =>
                {
                    _lens.scale.value = 1.0f - time_ * 0.85f / 0.15f;
                },
                () =>
                {
                    _lens.scale.value = 0.15f;
                });

            Initiate.Fade(Name.Scene.Game, Color.black, 1.0f);
        }

        StartCoroutine(Co());
    }

    void OnButton_Quit()
    {
        m_Audio.Play();
        m_Animator.SetTrigger("Off");
    }

    void OnButton_Option()
    {
        m_EscMenu.Switch();
    }

    void OnButton_Credits()
    {
        m_Audio.Play();
        OnMenuCredits?.Invoke();
    }

    #endregion Menu-Defalut
}