using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ExUnityEditor;

public class MapCreater : EditorWindow
{
    [SerializeField] static bool InitFlag = true;

    const int UIWidth = 300;
    readonly GUILayoutOption GUIWIDTH = GUILayout.Width(UIWidth);
    readonly GUILayoutOption GUIHEIGHT = GUILayout.Height(50);
    readonly Color UIColor = Color.cyan;
    readonly Color EditUIColor = Color.green;
    readonly int[,] renderSize;
    readonly string[] renderSizeStr;

    static GUIStyle style;

    static string[] editChipStrs, editObjStrs;
    static Texture[] editChipTexs, editObjTexs;

    [SerializeField] MapData_SO edit_SO;

    [SerializeField] static string ScriptPath = "Assets";
    [SerializeField] static string ChipPath = "Assets";
    [SerializeField] static string ObjPath = "Assets";

    [SerializeField] static int[,] editChip, editObj;
    [SerializeField] static Vector2Int
        editScaleNow = new(3, 3),
        editScaleNew = new(3, 3);
    [SerializeField] int editSizeIndex = 0;

    [SerializeField] bool autoSave = false;
    [SerializeField] bool pngMode = false;

    [SerializeField] int  selectChipIndex = 0, selectObjIndex = 0;

    [MenuItem("MapCreater/Open MapCreaterWindow")]
    static void Open()
    {
        MapCreater window = GetWindow<MapCreater>();
        window.minSize = new Vector2(500, 400);
        window.maxSize = new Vector2(1000, 800);
    }

    MapCreater()
    {
        renderSize = new int[4, 2] { { 32, 36 }, { 64, 68 }, { 96, 100 }, { 128, 172 } };
        renderSizeStr = new string[4] { "32", "64", "96", "128" };
    }
    
    static void Initialize()
    {
        InitFlag = false;

        style = new()
        {
            alignment = TextAnchor.MiddleCenter,
        };
        style.normal.textColor = Color.white;

        editChip = new int[editScaleNow.y, editScaleNow.x];
        editObj = new int[editScaleNow.y, editScaleNow.x];

        editChipStrs = new string[] { "Non", "Ground", "Damage" };
        editObjStrs = new string[] { "Non", "Box", "Unti" };

        editChipTexs = new Texture[editChipStrs.Length];
        editObjTexs = new Texture[editObjStrs.Length];

        ScriptPath = ExEditorUtility.GetScriptPath<MapCreater>(); ;
        ChipPath = ScriptPath + "/MapImages/Chip";
        ObjPath = ScriptPath + "/MapImages/Obj";

        for(int i = 0; i < editChipTexs.Length; ++i)
            editChipTexs[i] = AssetDatabase.LoadAssetAtPath<Texture>($"{ChipPath}/{editChipStrs[i]}.png");
        for(int i = 0; i < editObjTexs.Length; ++i)
            editObjTexs[i] = AssetDatabase.LoadAssetAtPath<Texture>($"{ObjPath}/{editObjStrs[i]}.png");
    }

    void OnGUI()
    {
        if (InitFlag) Initialize();
        
        //UI背景
        using (new ExColorScope.GUIBackGround(UIColor))
            GUI.Box(new Rect(0, 0, UIWidth + 5, position.size.y), "");

        //マップデータを受け取るフィールド
        edit_SO = EditorGUILayout.ObjectField("Mapデータ", edit_SO, typeof(MapData_SO), false, GUIWIDTH) as MapData_SO;
        using (new GUILayout.HorizontalScope(GUIWIDTH))
        {
            if(GUILayout.Button("new")) New();
            if(GUILayout.Button("Load")) Load();
            if(GUILayout.Button("Save")) Save();
        }

        //テキストフィールド(フォルダをドラッグでパスを受け取れる)
        var rect = GUILayoutUtility.GetRect(0, 18, GUI.skin.textField, GUIWIDTH);
        ScriptPath = GUI.TextField(rect, ScriptPath);
        CatchDragAndDrop(rect, ref ScriptPath);

        //サイズフィールド
        GUILayout.Label("編集サイズ", style, GUIWIDTH);
        editScaleNew = EditorGUILayout.Vector2IntField("", editScaleNew, GUIWIDTH);
        editScaleNew.x = Mathf.Max(editScaleNew.x, 1);
        editScaleNew.y = Mathf.Max(editScaleNew.y, 1);
        if (GUILayout.Button("サイズ変更", GUIWIDTH))
        {
            if(editScaleNow != editScaleNew)
                SizeChange();
        }
        GUILayout.Label("描画サイズ", style, GUIWIDTH);
        editSizeIndex = GUILayout.Toolbar(editSizeIndex, renderSizeStr, GUIWIDTH, GUIHEIGHT);

        //画像化ボタンとオートセーブボタン
        using(new GUILayout.HorizontalScope(GUIWIDTH))
        {
            pngMode = GUILayout.Toggle(pngMode, "画像モード");
            autoSave = GUILayout.Toggle(autoSave, "オートセーブ");
        }

        //編集アイテムボタン
        if (!pngMode)
        {
            selectChipIndex = GUILayout.Toolbar(selectChipIndex, editChipStrs, GUIWIDTH, GUIHEIGHT);
            selectObjIndex = GUILayout.Toolbar(selectObjIndex, editObjStrs, GUIWIDTH, GUIHEIGHT);
        }
        else
        {
            selectChipIndex = GUILayout.Toolbar(selectChipIndex, editChipTexs, GUIWIDTH, GUIHEIGHT);
            selectObjIndex = GUILayout.Toolbar(selectObjIndex, editObjTexs, GUIWIDTH, GUIHEIGHT);
        }

        //エディット背景
        using (new ExColorScope.GUIBackGround(EditUIColor))
            GUI.Box(new Rect(UIWidth + 10, 0, position.size.x - UIWidth - 10, position.size.y), "");

        //エディターチップ描画
        for (int y = 0; y < editScaleNow.y; ++y)
        {
            for (int x = 0; x < editScaleNow.x; ++x)
            {
                var r = new Rect(
                    x * renderSize[editSizeIndex, 0] + UIWidth + 10,
                    y * renderSize[editSizeIndex, 0],
                    renderSize[editSizeIndex, 1],
                    renderSize[editSizeIndex, 1]);
                GUI.Label(r, editChipTexs[editChip[y, x]]);
                GUI.Label(r, editObjTexs[editObj[y, x]]);
            }
        }

        //マウス入力処理
        Event e = Event.current;
        if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        {
            var mPos = new Vector2Int((int)e.mousePosition.x, (int)e.mousePosition.y);
            if(mPos.x >= UIWidth + 10 && mPos.x < UIWidth + 10 + editScaleNow.x * renderSize[editSizeIndex, 0])
            {
                if (mPos.y >= 0 && mPos.y < editScaleNow.y * renderSize[editSizeIndex, 0])
                {
                    int offset = renderSize[editSizeIndex, 0];

                    mPos.x -= UIWidth + 10;
                    mPos.x /= offset;
                    mPos.y /= offset;

                    editChip[mPos.y , mPos.x] = selectChipIndex;
                    editObj[mPos.y , mPos.x] = selectObjIndex;

                    Repaint();
                    if(autoSave) Save();
                }
            }
        }
    }

    /// <summary>
    /// ドラッグアンドドロップを確認する関数
    /// </summary>
    /// 参考元　https://kan-kikuchi.hatenablog.com/entry/PathAttribute_1
    void CatchDragAndDrop(Rect rect_, ref string path_)
    {
        //範囲内にマウスがあるときのみ開始
        if(rect_.Contains(Event.current.mousePosition))
        {
            EventType eventType = Event.current.type;

            if(eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                List<Object> list = new List<Object>();
                if(eventType == EventType.DragPerform)
                {
                    list = new List<Object>(DragAndDrop.objectReferences);
                    DragAndDrop.AcceptDrag();
                }
                Event.current.Use();
                if(list.Count > 0)
                    path_ = AssetDatabase.GetAssetPath(list[0]);
                InitFlag = true;
            }
        }
    }

    void SizeChange()
    {
        if(editScaleNew.x > 0 && editScaleNew.y > 0)
        {
            var preMap = editChip;
            var preObj = editObj;

            editChip = new int[editScaleNew.y, editScaleNew.x];
            editObj = new int[editScaleNew.y, editScaleNew.x];

            var yy = editScaleNow.y >= editScaleNew.y ? editScaleNew.y : editScaleNow.y;
            var xx = editScaleNow.x >= editScaleNew.x ? editScaleNew.x : editScaleNow.x;

            for(int y = 0; y < yy; ++y)
            {
                for(int x = 0; x < xx; ++x)
                {
                    editChip[y, x] = preMap[y, x];
                    editObj[y, x] = preObj[y, x];
                }
            }

            editScaleNow = editScaleNew;
        }
    }

    void New()
    {
        editScaleNow = new Vector2Int(10, 10);
        editChip = new int [editScaleNow.y, editScaleNow.x];
        editObj = new int[editScaleNow.y, editScaleNow.x];
    }

    void Load()
    {
        if(edit_SO == null)
        {
            return;
        }

        editScaleNow = new Vector2Int(edit_SO.x, edit_SO.y);
        editScaleNew = new Vector2Int(edit_SO.x, edit_SO.y);
        editChip = new int[editScaleNow.y, editScaleNow.x];
        editObj = new int[editScaleNow.y, editScaleNow.x];

        //値渡しと参照渡しで配列は参照を勝手に渡されてしまうのでコピーを行っている
        //ようは勝手に配列ポインターを相互参照するようになり
        //勝手にエディタとSOの配列データが常に同期し始める
        //System.Array.Copyでは対応できない
        for(int y = edit_SO.y - 1, yy = 0; y >= 0; --y, ++yy)
        //for(int y = 0; y < edit_SO.y; ++y)
        {
            for(int x = 0; x < edit_SO.x; ++x)
            {
                editChip[y, x] = edit_SO.mapChip[yy * editScaleNow.x + x] + 1;
                editObj[y, x] = edit_SO.objChip[yy * editScaleNow.x + x] + 1;
            }
        }
                
        Repaint();
    }

    void Save()
    {
        if(edit_SO == null)
        {
            return;
        }

        edit_SO.x = editScaleNow.x;
        edit_SO.y = editScaleNow.y;

        edit_SO.mapChip = new int[edit_SO.y * edit_SO.x];
        edit_SO.objChip = new int[edit_SO.y * edit_SO.x];

        for(int y = edit_SO.y - 1, yy = 0; y >= 0; --y, ++yy)
        //for (int y = 0; y < edit_SO.y; ++y)
        {
            for(int x = 0; x < edit_SO.x; ++x)
            {
                edit_SO.mapChip[yy * edit_SO.x + x] = editChip[y, x] -1;
                edit_SO.objChip[yy * edit_SO.x + x] = editObj[y, x] - 1;
            }
        }

        EditorUtility.SetDirty(edit_SO);
        AssetDatabase.SaveAssets();
    }
}