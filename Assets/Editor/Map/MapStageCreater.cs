using ExUnityEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Map.Stage;

/// <summary>
/// マップデータを編集するエディター(簡易版)クラス
/// </summary>
public class MapStageCreater : EditorWindow
{
    #region Field
    private const int UI_WIDTH = 300;
    private readonly GUILayoutOption m_GUI_WIDTH = GUILayout.Width(UI_WIDTH);
    private readonly GUILayoutOption m_GUI_HEIGHT = GUILayout.Height(50);
    private readonly Color m_UI_COLOR = Color.cyan;
    private readonly Color m_EDIT_UI_COLOR = Color.green;
    private readonly int[][] m_CHIP_RENDER_SIZE;
    private readonly string[] m_CHIP_RENDER_SIZE_STRING;

    private static bool s_isDIrty = true;

    private static GUIStyle s_style;

    private static string[] m_editMapChipName, m_editMapObjectName;
    private static Texture[] m_editMapChipTexture, m_editMapObjectTexture;

    private MapStage_SO m_editSO;

    private static string m_scriptPath = "Assets/Editor/Map";

    private static int[,] m_editMapChip, m_editMapObject;
    private static Vector2Int
        m_editScaleNow = new(10, 10),
        m_editScaleNew = new(10, 10);
    private int m_editSizeIndex = 0;

    private bool m_isAutoSave = false;
    private bool m_isPngMode = false;

    private int selectChipIndex = 0, selectObjIndex = 0;
    #endregion Field

    [MenuItem("BoB/Map/Open MapCreaterWindow")]
    private static void Open()
    {
        var _window = GetWindow<MapStageCreater>();
        _window.minSize = new Vector2(500, 320);
        _window.maxSize = new Vector2(1000, 800);
    }

    private MapStageCreater()
    {
        m_CHIP_RENDER_SIZE = new int[4][];
        for(int i = 0, len = m_CHIP_RENDER_SIZE.Length; i < len; ++i)
        {
            m_CHIP_RENDER_SIZE[i] = new int[2];
        }
        m_CHIP_RENDER_SIZE[0][0] = 32;
        m_CHIP_RENDER_SIZE[0][1] = 36;
        m_CHIP_RENDER_SIZE[1][0] = 64;
        m_CHIP_RENDER_SIZE[1][1] = 68;
        m_CHIP_RENDER_SIZE[2][0] = 96;
        m_CHIP_RENDER_SIZE[2][1] = 100;
        m_CHIP_RENDER_SIZE[3][0] = 128;
        m_CHIP_RENDER_SIZE[3][1] = 172;

        m_CHIP_RENDER_SIZE_STRING = new string[m_CHIP_RENDER_SIZE.Length];
        for(int i = 0, len = m_CHIP_RENDER_SIZE_STRING.Length; i < len; ++i)
        {
            m_CHIP_RENDER_SIZE_STRING[i] = m_CHIP_RENDER_SIZE[i][0].ToString();
        }
    }

    private static void Initialize()
    {
        s_isDIrty = false;

        s_style = new()
        {
            alignment = TextAnchor.MiddleCenter,
        };
        s_style.normal.textColor = Color.white;

        m_editMapChip = new int[m_editScaleNow.y, m_editScaleNow.x];
        m_editMapObject = new int[m_editScaleNow.y, m_editScaleNow.x];

        m_editMapChipName = new string[] { "Non", "Ground", "Damage" };
        m_editMapObjectName = new string[] { "Non", "Box", "Unti" };

        m_editMapChipTexture = new Texture[m_editMapChipName.Length];
        m_editMapObjectTexture = new Texture[m_editMapObjectName.Length];

        m_scriptPath = ExEditorUtility.GetScriptPath<MapStageCreater>();
        var _mapChipPath = m_scriptPath + "/Images/Chip";
        var _mapObjectPath = m_scriptPath + "/Images/Object";

        for(int i = 0; i < m_editMapChipTexture.Length; ++i)
            m_editMapChipTexture[i] = AssetDatabase.LoadAssetAtPath<Texture>($"{_mapChipPath}/{m_editMapChipName[i]}.png");
        for(int i = 0; i < m_editMapObjectTexture.Length; ++i)
            m_editMapObjectTexture[i] = AssetDatabase.LoadAssetAtPath<Texture>($"{_mapObjectPath}/{m_editMapObjectName[i]}.png");
    }

    private void OnGUI()
    {
        if(s_isDIrty) Initialize();

        // UI背景
        using(new ExColorScope.GUIBackGround(m_UI_COLOR))
            GUI.Box(new Rect(0, 0, UI_WIDTH + 5, position.size.y), "");

        // マップデータを受け取るフィールド
        m_editSO = EditorGUILayout.ObjectField("Mapデータ", m_editSO, typeof(MapStage_SO), false, m_GUI_WIDTH) as MapStage_SO;
        using(new GUILayout.HorizontalScope(m_GUI_WIDTH))
        {
            if(GUILayout.Button("new")) New();
            if(GUILayout.Button("Load")) Load();
            if(GUILayout.Button("Save")) Save();
        }

        {
            // テキストフィールド(フォルダをドラッグでパスを受け取れる)
            var _rect = GUILayoutUtility.GetRect(0, 18, GUI.skin.textField, m_GUI_WIDTH);
            m_scriptPath = GUI.TextField(_rect, m_scriptPath);
            CatchDragAndDrop(_rect, ref m_scriptPath);
        }

        // サイズフィールド
        GUILayout.Label("編集サイズ", s_style, m_GUI_WIDTH);
        m_editScaleNew = EditorGUILayout.Vector2IntField("", m_editScaleNew, m_GUI_WIDTH);
        m_editScaleNew.x = Mathf.Max(m_editScaleNew.x, 1);
        m_editScaleNew.y = Mathf.Max(m_editScaleNew.y, 1);
        if(GUILayout.Button("サイズ変更", m_GUI_WIDTH))
        {
            if(m_editScaleNow != m_editScaleNew)
                SizeChange();
        }
        GUILayout.Label("描画サイズ", s_style, m_GUI_WIDTH);
        m_editSizeIndex = GUILayout.Toolbar(m_editSizeIndex, m_CHIP_RENDER_SIZE_STRING, m_GUI_WIDTH, m_GUI_HEIGHT);

        // 画像化ボタンとオートセーブボタン もうすこし上手いこと改良できそう
        using(new GUILayout.HorizontalScope(m_GUI_WIDTH))
        {
            m_isPngMode = GUILayout.Toggle(m_isPngMode, "画像モード");
            m_isAutoSave = GUILayout.Toggle(m_isAutoSave, "オートセーブ");
        }

        // 編集アイテムボタン
        if(!m_isPngMode)
        {
            selectChipIndex = GUILayout.Toolbar(selectChipIndex, m_editMapChipName, m_GUI_WIDTH, m_GUI_HEIGHT);
            selectObjIndex = GUILayout.Toolbar(selectObjIndex, m_editMapObjectName, m_GUI_WIDTH, m_GUI_HEIGHT);
        }
        else
        {
            selectChipIndex = GUILayout.Toolbar(selectChipIndex, m_editMapChipTexture, m_GUI_WIDTH, m_GUI_HEIGHT);
            selectObjIndex = GUILayout.Toolbar(selectObjIndex, m_editMapObjectTexture, m_GUI_WIDTH, m_GUI_HEIGHT);
        }

        // エディット背景
        using(new ExColorScope.GUIBackGround(m_EDIT_UI_COLOR))
            GUI.Box(new Rect(UI_WIDTH + 10, 0, position.size.x - UI_WIDTH - 10, position.size.y), "");

        // エディターチップ描画
        for(int y = 0; y < m_editScaleNow.y; ++y)
        {
            for(int x = 0; x < m_editScaleNow.x; ++x)
            {
                var _rect = new Rect(
                    x * m_CHIP_RENDER_SIZE[m_editSizeIndex][0] + UI_WIDTH + 10,
                    y * m_CHIP_RENDER_SIZE[m_editSizeIndex][0],
                    m_CHIP_RENDER_SIZE[m_editSizeIndex][1],
                    m_CHIP_RENDER_SIZE[m_editSizeIndex][1]);
                GUI.Label(_rect, m_editMapChipTexture[m_editMapChip[y, x]]);
                GUI.Label(_rect, m_editMapObjectTexture[m_editMapObject[y, x]]);
            }
        }

        // マウス入力処理
        Event _event = Event.current;
        if(_event.type == EventType.MouseDown || _event.type == EventType.MouseDrag)
        {
            var _mPos = new Vector2Int((int)_event.mousePosition.x, (int)_event.mousePosition.y);
            if(_mPos.x >= UI_WIDTH + 10 && _mPos.x < UI_WIDTH + 10 + m_editScaleNow.x * m_CHIP_RENDER_SIZE[m_editSizeIndex][0])
            {
                if(_mPos.y >= 0 && _mPos.y < m_editScaleNow.y * m_CHIP_RENDER_SIZE[m_editSizeIndex][0])
                {
                    int offset = m_CHIP_RENDER_SIZE[m_editSizeIndex][0];

                    _mPos.x -= UI_WIDTH + 10;
                    _mPos.x /= offset;
                    _mPos.y /= offset;

                    m_editMapChip[_mPos.y, _mPos.x] = selectChipIndex;
                    m_editMapObject[_mPos.y, _mPos.x] = selectObjIndex;

                    Repaint();
                    if(m_isAutoSave) Save();
                }
            }
        }
    }

    /// <summary>
    /// ドラッグアンドドロップを確認する関数
    /// </summary>
    /// 参考元　https://kan-kikuchi.hatenablog.com/entry/PathAttribute_1
    private void CatchDragAndDrop(Rect rect_, ref string path_)
    {
        // 範囲内にマウスがあるときのみ開始
        if(rect_.Contains(Event.current.mousePosition))
        {
            EventType eventType = Event.current.type;

            if(eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                var list = new List<Object>();
                if(eventType == EventType.DragPerform)
                {
                    list = new(DragAndDrop.objectReferences);
                    DragAndDrop.AcceptDrag();
                }
                Event.current.Use();
                if(list.Count > 0)
                    path_ = AssetDatabase.GetAssetPath(list[0]);
                s_isDIrty = true;
            }
        }
    }

    private void SizeChange()
    {
        if(m_editScaleNew.x > 0 && m_editScaleNew.y > 0)
        {
            var preMap = m_editMapChip;
            var preObj = m_editMapObject;

            m_editMapChip = new int[m_editScaleNew.y, m_editScaleNew.x];
            m_editMapObject = new int[m_editScaleNew.y, m_editScaleNew.x];

            var yy = m_editScaleNow.y >= m_editScaleNew.y ? m_editScaleNew.y : m_editScaleNow.y;
            var xx = m_editScaleNow.x >= m_editScaleNew.x ? m_editScaleNew.x : m_editScaleNow.x;

            for(int y = 0; y < yy; ++y)
            {
                for(int x = 0; x < xx; ++x)
                {
                    m_editMapChip[y, x] = preMap[y, x];
                    m_editMapObject[y, x] = preObj[y, x];
                }
            }

            m_editScaleNow = m_editScaleNew;
        }
    }

    private void New()
    {
        m_editScaleNow = new Vector2Int(10, 10);
        m_editMapChip = new int[m_editScaleNow.y, m_editScaleNow.x];
        m_editMapObject = new int[m_editScaleNow.y, m_editScaleNow.x];
    }

    private void Load()
    {
        if(m_editSO == null) return;

        m_editScaleNow = m_editSO.Size;
        m_editScaleNew = m_editSO.Size;

        m_editMapChip = new int[m_editScaleNow.y, m_editScaleNow.x];
        m_editMapObject = new int[m_editScaleNow.y, m_editScaleNow.x];

        // 値渡しと参照渡しで配列は参照を勝手に渡されてしまうのでコピーを行っている
        // ようは勝手に配列ポインターを相互参照するようになり
        // 勝手にエディタとSOの配列データが常に同期し始める
        // System.Array.Copyでは対応できない
        for(int y = 0; y < m_editSO.Size.y; ++y)
        {
            int _topY = m_editSO.Size.y - 1 - y;
            for(int x = 0; x < m_editSO.Size.x; ++x)
            {
                m_editMapChip[y, x] = m_editSO.MapChip[_topY * m_editScaleNow.x + x] + 1;
                m_editMapObject[y, x] = m_editSO.MapObject[_topY * m_editScaleNow.x + x] + 1;
            }
        }

        Repaint();
    }

    private void Save()
    {
        if(m_editSO == null) return;

        m_editSO.Size = m_editScaleNow;

        m_editSO.MapChip = new int[m_editSO.MapSize];
        m_editSO.MapObject = new int[m_editSO.MapSize];

        // 上方向にY+のXY軸のマップデータにするための軸変換　保存データは下方向にY+
        for(int y = 0; y < m_editSO.Size.y; ++y)
        {
            int _topY = m_editSO.Size.y - 1 - y;
            for(int x = 0; x < m_editSO.Size.x; ++x)
            {
                // Nonは-1とするためのずらし-1
                m_editSO.MapChip[_topY * m_editSO.Size.x + x] = m_editMapChip[y, x] - 1;
                m_editSO.MapObject[_topY * m_editSO.Size.x + x] = m_editMapObject[y, x] - 1;
            }
        }

        EditorUtility.SetDirty(m_editSO);
        AssetDatabase.SaveAssets();
    }
}