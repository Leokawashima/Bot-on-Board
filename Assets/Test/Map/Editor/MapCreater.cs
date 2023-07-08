using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ExUnityEditor;
using UnityEditor.Callbacks;

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

    static string[] editMapStrs, editObjStrs;
    static Texture[] editMapTexs, editObjTexs;

    [SerializeField] static Data_SO edit_SO;

    [SerializeField] static string path = "Assets/Test/Map/image";

    [SerializeField] static int[,] editMap, editObj;
    [SerializeField] static Vector2Int
        editScaleNow = new Vector2Int(10, 10),
        editScaleNew = new Vector2Int(10, 10);
    [SerializeField] static int editSizeIndex = 0;

    [SerializeField] static bool autoSave = false;
    [SerializeField] static bool pngMode = false;

    [SerializeField]
    static int  selectMapIndex = 0, selectObjIndex = 0;

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
    
    [DidReloadScripts]
    static void Initialize()
    {
        InitFlag = false;

        //�t�H���g�X�^�C��
        style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;

        editMap = new int[editScaleNow.y, editScaleNow.x];
        editObj = new int[editScaleNow.y, editScaleNow.x];

        editMapStrs = System.Enum.GetNames(typeof(MapManager.MapState));
        editObjStrs = System.Enum.GetNames(typeof(MapManager.ObjState));

        editMapTexs = new Texture[System.Enum.GetValues(typeof(MapManager.MapState)).Length];
        editObjTexs = new Texture[System.Enum.GetValues(typeof(MapManager.ObjState)).Length];

        for(int i = 0; i < editMapTexs.Length; i++)
            editMapTexs[i] = AssetDatabase.LoadAssetAtPath<Texture>($"{path}/{editMapStrs[i]}.png");
        for(int i = 0; i < editObjTexs.Length; i++)
            editObjTexs[i] = AssetDatabase.LoadAssetAtPath<Texture>($"{path}/{editObjStrs[i]}.png");
    }

    void OnGUI()
    {
        if (InitFlag) Initialize();

        //UI�w�i
        using (new ExColorScope.GUIBackGround(UIColor))
            GUI.Box(new Rect(0, 0, UIWidth + 5, position.size.y), "");

        //�}�b�v�f�[�^���󂯎��t�B�[���h
        edit_SO = EditorGUILayout.ObjectField("Map�f�[�^", edit_SO, typeof(Data_SO), false, GUIWIDTH) as Data_SO;
        using (new GUILayout.HorizontalScope(GUIWIDTH))
        {
            if(GUILayout.Button("new")) New();
            if(GUILayout.Button("Load")) Load();
            if(GUILayout.Button("Save")) Save();
        }

        //�e�L�X�g�t�B�[���h(�t�H���_���h���b�O�Ńp�X���󂯎���)
        var rect = GUILayoutUtility.GetRect(0, 18, GUI.skin.textField, GUIWIDTH);
        path = GUI.TextField(rect, path);
        CatchDragAndDrop(rect, ref path);

        //�T�C�Y�t�B�[���h
        GUILayout.Label("�ҏW�T�C�Y", style, GUIWIDTH);
        editScaleNew = EditorGUILayout.Vector2IntField("", editScaleNew, GUIWIDTH);
        editScaleNew.x = Mathf.Max(editScaleNew.x, 1);
        editScaleNew.y = Mathf.Max(editScaleNew.y, 1);
        if (GUILayout.Button("�T�C�Y�ύX", GUIWIDTH))
        {
            if(editScaleNow != editScaleNew)
                SizeChange();
        }
        GUILayout.Label("�`��T�C�Y", style, GUIWIDTH);
        editSizeIndex = GUILayout.Toolbar(editSizeIndex, renderSizeStr, GUIWIDTH, GUIHEIGHT);

        //�摜���{�^���ƃI�[�g�Z�[�u�{�^��
        using(new GUILayout.HorizontalScope(GUIWIDTH))
        {
            pngMode = GUILayout.Toggle(pngMode, "�摜���[�h");
            autoSave = GUILayout.Toggle(autoSave, "�I�[�g�Z�[�u");
        }

        //�ҏW�A�C�e���{�^��
        if (!pngMode)
        {
            selectMapIndex = GUILayout.Toolbar(selectMapIndex, editMapStrs, GUIWIDTH, GUIHEIGHT);
            selectObjIndex = GUILayout.Toolbar(selectObjIndex, editObjStrs, GUIWIDTH, GUIHEIGHT);
        }
        else
        {
            selectMapIndex = GUILayout.Toolbar(selectMapIndex, editMapTexs, GUIWIDTH, GUIHEIGHT);
            selectObjIndex = GUILayout.Toolbar(selectObjIndex, editObjTexs, GUIWIDTH, GUIHEIGHT);
        }

        //�G�f�B�b�g�w�i
        using (new ExColorScope.GUIBackGround(EditUIColor))
            GUI.Box(new Rect(UIWidth + 10, 0, position.size.x - UIWidth - 10, position.size.y), "");

        //�G�f�B�^�[�`�b�v�`��
        for (int y = 0; y < editScaleNow.y; ++y)
        {
            for (int x = 0; x < editScaleNow.x; ++x)
            {
                var r = new Rect(
                    x * renderSize[editSizeIndex, 0] + UIWidth + 10,
                    y * renderSize[editSizeIndex, 0],
                    renderSize[editSizeIndex, 1],
                    renderSize[editSizeIndex, 1]);
                GUI.Label(r, editMapTexs[editMap[y, x]]);
                GUI.Label(r, editObjTexs[editObj[y, x]]);
            }
        }

        //�}�E�X���͏���
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

                    editMap[mPos.y , mPos.x] = selectMapIndex;
                    editObj[mPos.y , mPos.x] = selectObjIndex;

                    Repaint();
                    if(autoSave) Save();
                }
            }
        }
    }

    /// <summary>
    /// �h���b�O�A���h�h���b�v���m�F����֐�
    /// </summary>
    /// �Q�l���@https://kan-kikuchi.hatenablog.com/entry/PathAttribute_1
    void CatchDragAndDrop(Rect rect_, ref string path_)
    {
        //�͈͓��Ƀ}�E�X������Ƃ��̂݊J�n
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
            var preMap = editMap;
            var preObj = editObj;

            editMap = new int[editScaleNew.y, editScaleNew.x];
            editObj = new int[editScaleNew.y, editScaleNew.x];

            var yy = editScaleNow.y >= editScaleNew.y ? editScaleNew.y : editScaleNow.y;
            var xx = editScaleNow.x >= editScaleNew.x ? editScaleNew.x : editScaleNow.x;

            for(int y = 0; y < yy; ++y)
            {
                for(int x = 0; x < xx; ++x)
                {
                    editMap[y, x] = preMap[y, x];
                    editObj[y, x] = preObj[y, x];
                }
            }

            editScaleNow = editScaleNew;
        }
    }

    void New()
    {
        editScaleNow = new Vector2Int(10, 10);
        editMap = new int [editScaleNow.y, editScaleNow.x];
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
        editMap = new int[editScaleNow.y, editScaleNow.x];
        editObj = new int[editScaleNow.y, editScaleNow.x];
        //�l�n���ƎQ�Ɠn���Ŕz��͎Q�Ƃ�����ɓn����Ă��܂��̂ŃR�s�[���s���Ă���
        //�悤�͏���ɔz��|�C���^�[�𑊌ݎQ�Ƃ���悤�ɂȂ�
        //����ɃG�f�B�^��SO�̔z��f�[�^����ɓ������n�߂�
        //System.Array.Copy�ł͑Ή��ł��Ȃ�
        for(int y = 0; y < edit_SO.y; ++y)
        {
            for(int x = 0; x < edit_SO.x; ++x)
            {
                editMap[y, x] = edit_SO.mapChip[y * editScaleNow.x + x];
                editObj[y, x] = edit_SO.objChip[y * editScaleNow.x + x];
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

        for (int y = 0; y < edit_SO.y; ++y)
        {
            for(int x = 0; x < edit_SO.x; ++x)
            {
                edit_SO.mapChip[y * edit_SO.x + x] = editMap[y, x];
                edit_SO.objChip[y * edit_SO.x + x] = editObj[y, x];
            }
        }

        EditorUtility.SetDirty(edit_SO);
        AssetDatabase.SaveAssets();
    }
}