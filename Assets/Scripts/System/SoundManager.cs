using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private static AudioTable_SO m_table;
    public static AudioTable_SO s_singleton = GetInstance(m_table);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetInstance<T>(T instance_) where T : class
    {
        if (instance_ != null)
        {
            return instance_;
        }

        var _asset = Resources.Load(typeof(T).Name) as T;
        if (_asset == null)
        {
#if UNITY_EDITOR
            Debug.LogError($"Missing Table for Serach Path from {typeof(T).Name}");
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // アプリで読み込めない場合強制終了
            Application.Quit();
#endif
        }

        instance_ = _asset;

        return instance_;
    }
}