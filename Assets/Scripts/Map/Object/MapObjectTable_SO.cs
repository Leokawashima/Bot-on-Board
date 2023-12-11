using System;
using UnityEngine;

namespace Map.Object
{
    [CreateAssetMenu(fileName = "MapObjectTable", menuName = "BoB/Map/Table/MapObjectTable")]
    public class MapObjectTable_SO : ScriptableObject
    {
        public MapObject_SO[] Data;

        private static readonly string RESOURCE_PATH = "MapObjectTable";

        private static MapObjectTable_SO m_instance = null;
        public static MapObjectTable_SO Instance
        {
            get
            {
                if(m_instance != null)
                {
                    return m_instance;
                }

                var _asset = Resources.Load(RESOURCE_PATH) as MapObjectTable_SO;
                if(_asset == null)
                {
#if UNITY_EDITOR
                    Debug.AssertFormat(false, "Missing ParameterTable! path={0}", RESOURCE_PATH);
                    _asset = Activator.CreateInstance<MapObjectTable_SO>();
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