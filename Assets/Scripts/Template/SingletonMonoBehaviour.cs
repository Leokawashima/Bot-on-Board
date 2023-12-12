using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>, new()
{
    public static T Singleton { get; private set; }

    protected virtual void Awake()
    {
        if(Singleton == null)
        {
            Singleton = (T)this;
        }
        else
        {
            Destroy(this);
        }
    }
    protected virtual void OnDestroy()
    {
        if(Singleton == this)
        {
            Singleton = null;
        }
    }
}