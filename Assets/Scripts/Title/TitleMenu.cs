using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TitleMenu : MonoBehaviour
{
    [Header("MenuParent")]
    [SerializeField] GameObject m_menuParent;
    [Header("Button")]
    [SerializeField] Button m_playButton;
    [SerializeField] Button m_quitButton;
    [SerializeField] Button m_optionButton;
    [SerializeField] Button m_creditButton;
    [SerializeField] Button m_tutorialButton;
    [Header("Sound")]
    [SerializeField] AudioSource m_audio;
    [Header("QuitAnim")]
    [SerializeField] AnimatorSystem m_animator;
    [Header("Profile")]
    [SerializeField] Volume m_volume;
    [Header("EscMenu")]
    [SerializeField] EscMenuManager m_EscMenu;

    public event Action OnShowCredit;
    public event Action OnShowTutorial;

    void OnEnable()
    {
        EscMenuManager.Event_Open += Disable;
        EscMenuManager.Event_Close += Enable;
    }
    void OnDisable()
    {
        EscMenuManager.Event_Open -= Disable;
        EscMenuManager.Event_Close -= Enable;
    }

    public void Initialize()
    {
        m_playButton.onClick.AddListener(OnButtonPlay);
        m_quitButton.onClick.AddListener(OnButtonQuit);
        m_optionButton.onClick.AddListener(OnButtonOption);
        m_creditButton.onClick.AddListener(OnButtonCredit);
        m_tutorialButton.onClick.AddListener(OnButtonTutorial);
    }

    public void Enable()
    {
        m_menuParent.SetActive(true);
    }
    public void Disable()
    {
        m_menuParent.SetActive(false);
    }

    #region Menu

    void OnButtonPlay()
    {
        m_audio.Play();
        m_menuParent.SetActive(false);

        StartCoroutine(Co_GoGameAnimation());

        IEnumerator Co_GoGameAnimation()
        {
            m_volume.enabled = true;

            m_volume.profile.TryGet(out LensDistortion _lens);
            m_volume.profile.TryGet(out Vignette _vignette);
            // 値初期化
            _lens.intensity.value = 0.3f;
            _lens.scale.value = 1.0f;
            _vignette.intensity.value = 0;

            // Profileの値をアニメーションで設定することができなかったので
            // コルーチンを使用して実装している
            yield return Co_Trasition(1.0f,
                (float elapsedTime_) =>
                {
                    _lens.intensity.value = 0.3f + elapsedTime_ * 0.4f;
                    _vignette.intensity.value = elapsedTime_ * 0.5f;
                },
                () =>
                {
                    _lens.intensity.value = 0.7f;
                    _vignette.intensity.value = 0.5f;
                });

            yield return Co_Trasition(0.05f,
                (float elapsedTime_) =>
                {
                    _lens.intensity.value = 0.7f - elapsedTime_ * 1.7f / 0.05f;
                },
                () =>
                {
                    _lens.intensity.value = -1.0f;
                });

            yield return Co_Trasition(0.15f,
                (float elapsedTime_) =>
                {
                    _lens.scale.value = 1.0f - elapsedTime_ * 0.85f / 0.15f;
                },
                () =>
                {
                    _lens.scale.value = 0.15f;
                });

            Initiate.Fade(Name.Scene.Game, Color.black, 1.0f);

            /// <summary>
            /// 経過時間を引数に呼ばれる処理を一定時間行って終了時にコールバックを呼び出すコルーチン
            /// </summary>
            /// <param name= "time_">処理する時間</param>
            IEnumerator Co_Trasition(float time_, Action<float> update_, Action completed_)
            {
                // 経過時間
                float _erapsedTime = 0;
                while(true)
                {
                    _erapsedTime += Time.deltaTime;
                    // 指定時間がすぎたら終了コールバックを呼んで終了
                    if(_erapsedTime > time_)
                    {
                        completed_();
                        yield break;
                    }
                    // 経過時間を引数に更新コールバックを呼び出す
                    update_(_erapsedTime);
                    yield return null;
                }
            }
        }
    }

    void OnButtonQuit()
    {
        m_audio.Play();
        m_animator.Play("Off", () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        });
    }

    void OnButtonOption()
    {
        m_EscMenu.Switch();
    }

    void OnButtonCredit()
    {
        m_audio.Play();
        OnShowCredit?.Invoke();
    }

    void OnButtonTutorial()
    {
        m_audio.Play();
        OnShowTutorial?.Invoke();
    }

    #endregion Menu
}