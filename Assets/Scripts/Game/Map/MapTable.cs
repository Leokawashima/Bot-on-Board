using System.Runtime.CompilerServices;
using UnityEngine;
using Map.Chip;
using Map.Object;
using Map.Stage;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Map
{
    public static class MapTable
    {
        private static MapStageTable_SO s_stage;
        public static MapStageTable_SO Stage => s_stage.GetInstance();

        private static MapChipTable_SO s_chip;
        public static MapChipTable_SO Chip => s_chip.GetInstance();

        private static MapObjectTable_SO s_object;
        public static MapObjectTable_SO Object => s_object.GetInstance();

        public static void Clear()
        {
            s_stage = null;
            s_chip = null;
            s_object = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T GetInstance<T>(this T instance_) where T : class
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

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void OnInitializeOnLoad()
        {
            EditorApplication.playModeStateChanged += (PlayModeStateChange state_) =>
            {
                if (state_ == PlayModeStateChange.ExitingPlayMode)
                {
                    Clear();
                }
            };
        }
#endif
    }
}