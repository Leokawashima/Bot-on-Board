using System;
using UnityEngine;

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
    [SerializeField] private PageWindowManager m_pageWindowManager;

    public static void Initialize() => Singleton.m_pageWindowManager.Initialize();
  
    public static void Enable(int index_) => Singleton.m_pageWindowManager.Enable(index_);
    public static void Enable(int index_, Action calback_)
    {
        Singleton.m_pageWindowManager.Event_Closed += OnClosed;
        void OnClosed()
        {
            calback_();
            Singleton.m_pageWindowManager.Event_Closed -= OnClosed;
        }

        Enable(index_);
    }
}