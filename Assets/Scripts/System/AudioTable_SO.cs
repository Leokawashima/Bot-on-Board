using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioTable", menuName = "BoB/System/AduioTable")]
public class AudioTable_SO : ScriptableObject
{
    [Serializable]
    public class Audio
    {
        /// <summary>
        /// 再生するクリップ
        /// </summary>
        [field: SerializeField]
        public AudioClip Clip { get; private set; }
        /// <summary>
        /// 再生音量
        /// </summary>
        [field: SerializeField]
        public float Volume { get; private set; } = 1.0f;
        /// <summary>
        /// 再生先のミキサーグループ
        /// </summary>
        [field: SerializeField]
        public AudioMixerGroup Group { get; private set; }
    }

    [field: SerializeField]
    public Audio[] AudioTable { get; private set; }
}