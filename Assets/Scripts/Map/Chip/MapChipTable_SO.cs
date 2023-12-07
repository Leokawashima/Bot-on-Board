using System;
using UnityEngine;

namespace Map.Chip
{
    [CreateAssetMenu(menuName = "Map/Table/MC_Table_SO")]
    public class MapChipTable_SO : ScriptableObject
    {
        public MapChip_SO[] Table;

        private static readonly string RESOURCE_PATH = "MapChipTable";

        private static MapChipTable_SO m_instance = null;
        public static MapChipTable_SO Instance
        {
            get
            {
                if(m_instance != null)
                {
                    return m_instance;
                }

                var _asset = Resources.Load(RESOURCE_PATH) as MapChipTable_SO;
                if(_asset == null)
                {
#if UNITY_EDITOR
                    Debug.AssertFormat(false, "Missing ParameterTable! path={0}", RESOURCE_PATH);
                    _asset = Activator.CreateInstance<MapChipTable_SO>();
#else
                // アプリのほうで読み込めない場合強制終了
                Application.Quit();
#endif
                }

                m_instance = _asset;

                return m_instance;
            }
        }

    }
}