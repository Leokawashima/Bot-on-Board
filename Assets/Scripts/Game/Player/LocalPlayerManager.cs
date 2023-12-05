using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerManager : MonoBehaviour
{
    public static LocalPlayerManager Singleton { get; private set; }

    public MapChip SelectChip { get; private set; }
    Vector2 m_mousePosition;

    (Color baseColor, Color outLineColor) m_selectColor;
    Coroutine m_activeCorutine;

    void OnEnable()
    {
        PlayerInputManager.OnMouseMainClickEvent += OnMouse_MainClick;
        PlayerInputManager.OnDragStartEvent += OnMouse_DragStart;
        PlayerInputManager.OnDragCancelEvent += OnMouse_DragCancel;
        GUIManager.Event_ButtonPlace += OnButton_Place;
    }
    void OnDisable()
    {
        PlayerInputManager.OnMouseMainClickEvent -= OnMouse_MainClick;
        PlayerInputManager.OnDragStartEvent -= OnMouse_DragStart;
        PlayerInputManager.OnDragCancelEvent -= OnMouse_DragCancel;
        GUIManager.Event_ButtonPlace -= OnButton_Place;
    }

    void Awake()
    {
        Singleton ??= this;
    }
    void OnDestroy()
    {
        Singleton = null;
    }

    void OnMouse_MainClick()
    {
        Stop();

        Ray _ray = Camera.main.ScreenPointToRay(PlayerInputManager.m_Pos);

        int _mask = 1 << Name.Layer.Map;

        // UI上なら返す　Updateで呼び出さないと不正確だぞ！とwarningが出るがいったん無視
        if (false == UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())

        if(Physics.Raycast(_ray, out var hit, Mathf.Infinity, _mask))
        {
            var _chip = hit.collider.GetComponent<MapChip>();
            if(SelectChip != _chip)
            {
                SelectChip = _chip;
                HighLight(SelectChip);
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
        if((m_mousePosition - PlayerInputManager.m_Pos).magnitude >= 20.0f)
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
            SelectChip.Material.color = m_selectColor.baseColor;
            SelectChip.Material.SetColor("_OutlineColor", m_selectColor.outLineColor);
        }
    }
    IEnumerator CoHighLight(MapChip chip_)
    {
        m_selectColor.baseColor = chip_.Material.color;
        m_selectColor.outLineColor = chip_.Material.GetColor("_OutlineColor");

        float _h, _v;
        Color.RGBToHSV(chip_.Material.color, out _h, out _, out _v);

        chip_.Material.SetColor("_OutlineColor", Color.blue);

        while (true)
        {
            chip_.Material.color = Color.HSVToRGB(_h, Time.time % 1, _v);
            yield return new WaitForSeconds(0.1f);
        }
    }
}