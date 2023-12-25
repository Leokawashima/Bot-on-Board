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
        public static MapStageTable_SO Stage => s_stage.GetInstance(RESOURCE_PATH[0]);

        private static MapChipTable_SO s_chip;
        public static MapChipTable_SO Chip => s_chip.GetInstance(RESOURCE_PATH[1]);

        private static MapObjectTable_SO s_object;
        public static MapObjectTable_SO Object => s_object.GetInstance(RESOURCE_PATH[2]);

        private static CardRarityTable_SO s_rarity;
        public static CardRarityTable_SO Rarity => s_rarity.GetInstance(RESOURCE_PATH[3]);

        private static CardCategoryTable_SO s_category;
        public static CardCategoryTable_SO Category => s_category.GetInstance(RESOURCE_PATH[4]);

        private static readonly string[] RESOURCE_PATH = new string[]
        {
            "MSTable",
            "MCTable",
            "MOTable",
            "RarityTable",
            "CategoryTable"
        };

        public static void Clear()
        {
            s_stage = null;
            s_chip = null;
            s_object = null;
            s_rarity = null;
            s_category = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T GetInstance<T>(this T instance_, string path_) where T : class
        {
            if (instance_ != null)
            {
                return instance_;
            }

            var _asset = Resources.Load(path_) as T;
            if (_asset == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Missing Table for Serach Path from {path_}");
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