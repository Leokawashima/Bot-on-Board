using UnityEngine;
using Map.Chip;
using Map.Object;
using Map.Stage;

namespace Map.Table
{
    public static class MapTable
    {
        private static MapStageTable_SO s_instanceStage;
        public static MapStageTable_SO Stage => s_instanceStage.GetInstance(RESOURCE_PATH[0]);

        public static MapChipTable_SO Chip { get; private set; }
        public static MapObjectTable_SO Object { get; private set; }

        private static readonly string[] RESOURCE_PATH = new string[]
        {
            "MSTable",
            "MCTable",
            "MOTable",
        };

        public static void Clear()
        {
            s_instanceStage = null;
            s_instanceStage = null;
            s_instanceStage = null;

#if UNITY_EDITOR
            Debug.Log("MapTable Called Clear");
#endif
        }

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
    }
}