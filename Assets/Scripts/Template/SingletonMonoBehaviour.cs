using System;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>, new()
{
    public static T Singleton { get; private set; }
    public static event Action Event_SingletonCreated;

    protected virtual void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this as T;
            Event_SingletonCreated?.Invoke();
        }
        else
        {
            Destroy(this);
        }
    }
    protected virtual void OnDestroy()
    {
        if (Singleton == this)
        {
            Singleton = null;
        }
    }
}