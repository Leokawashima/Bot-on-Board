using System.Collections;
using UnityEngine;
using Map.Chip;

public class LocalPlayerManager : SingletonMonoBehaviour<LocalPlayerManager>
{
    public MapChip SelectChip { get; private set; }
    Vector2 m_mousePosition;

    private bool m_isPointerOver;

    (Color main, Color outLine) m_selectColor;
    Coroutine m_activeCorutine;

    void OnEnable()
    {
        PlayerInputManager.OnMouseMainClickEvent += OnMouse_MainClick;
        PlayerInputManager.OnDragStartEvent += OnMouse_DragStart;
        PlayerInputManager.OnDragCancelEvent += OnMouse_DragCancel;
        PlayerUIManager.Event_ButtonPlace += OnButton_Place;
    }
    void OnDisable()
    {
        PlayerInputManager.OnMouseMainClickEvent -= OnMouse_MainClick;
        PlayerInputManager.OnDragStartEvent -= OnMouse_DragStart;
        PlayerInputManager.OnDragCancelEvent -= OnMouse_DragCancel;
        PlayerUIManager.Event_ButtonPlace -= OnButton_Place;
    }

    private void Update()
    {
        m_isPointerOver = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    void OnMouse_MainClick()
    {
        Ray _ray = Camera.main.ScreenPointToRay(PlayerInputManager.m_Pos);

        int _mask = 1 << Name.Layer.Map;

        // UI上かどうか
        if (false == m_isPointerOver)
        {
            if (Physics.Raycast(_ray, out var hit, Mathf.Infinity, _mask))
            {
                var _chip = hit.collider.GetComponent<MapChip>();
                if (SelectChip != _chip)
                {
                    Stop();
                    SelectChip = _chip;
                    HighLight(SelectChip);
                }
            }
        }
    }

    void OnMouse_DragStart()
    {
        m_mousePosition = PlayerInputManager.m_Pos;
        PlayerInputManager.OnMouseMovePerformEvent += OnMouse_MovePerform;
    }

    void OnMouse_DragCancel()
    {
        PlayerInputManager.OnMouseMovePerformEvent -= OnMouse_MovePerform;
        CameraManager.Singleton.SetFreeLookCamIsMove(false);
    }

    void OnMouse_MovePerform()
    {
        if ((m_mousePosition - PlayerInputManager.m_Pos).magnitude >= 20.0f)
        {
            CameraManager.Singleton.SetFreeLookCamIsMove(true);
            PlayerInputManager.OnMouseMovePerformEvent -= OnMouse_MovePerform;
        }
    }

    void OnButton_Place()
    {
        Stop();
    }

    private void HighLight(MapChip chip_)
    {
        m_activeCorutine = StartCoroutine(CoHighLight(chip_));
    }
    private void Stop()
    {
        if (m_activeCorutine != null)
        {
            StopCoroutine(m_activeCorutine);
            m_activeCorutine = null;
            SelectChip.Material.color = m_selectColor.main;
            // SelectChip.Material.SetColor("_OutlineColor", m_selectColor.outLine);
        }
    }
    IEnumerator CoHighLight(MapChip chip_)
    {
        m_selectColor.main = chip_.Material.color;
        // m_selectColor.outLine = chip_.Material.GetColor("_OutlineColor");

        float _h, _v;
        Color.RGBToHSV(chip_.Material.color, out _h, out _, out _v);

        // chip_.Material.SetColor("_OutlineColor", Color.blue);

        while (true)
        {
            chip_.Material.color = Color.HSVToRGB(_h, Time.time % 1, _v);
            yield return new WaitForSeconds(0.1f);
        }
    }
}