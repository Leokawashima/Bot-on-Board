using UnityEngine;
using Map;
using Map.Chip;

public class LocalPlayerManager : SingletonMonoBehaviour<LocalPlayerManager>
{
    public MapChip SelectChip { get; private set; }
    private Vector2 m_mousePosition;

    private bool m_isPointerOver;

    private readonly int m_mask = 1 << Name.Layer.Map;

    private void OnEnable()
    {
        InputManager.Event_Main += OnMain;
        InputManager.Event_DragStart += OnDragStart;
        InputManager.Event_DragCancel += OnDragCancel;
        PlayerUIManager.Event_ButtonPlace += OnButton_Place;
    }
    private void OnDisable()
    {
        InputManager.Event_Main -= OnMain;
        InputManager.Event_DragStart -= OnDragStart;
        InputManager.Event_DragCancel -= OnDragCancel;
        PlayerUIManager.Event_ButtonPlace -= OnButton_Place;
    }

    private void Update()
    {
        m_isPointerOver = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void OnMain()
    {
        var _ray = Camera.main.ScreenPointToRay(InputManager.Position);

        if (false == m_isPointerOver)
        {
            if (Physics.Raycast(_ray, out var hit, Mathf.Infinity, m_mask))
            {
                var _mapManager = MapManager.Singleton;
                var _pos = new Vector2Int((int)(hit.point.x + _mapManager.Stage.Size.x / 2.0f), (int)(hit.point.z + _mapManager.Stage.Size.y / 2.0f));
                var _chip = MapManager.Singleton.Stage.Chip[_pos.y][_pos.x];
                if (SelectChip != _chip)
                {
                    SelectChip = _chip;
                    var _select = MapManager.Singleton.Select;
                    _select.enabled = true;
                    _select.transform.localPosition = new(_chip.transform.localPosition.x, _select.transform.localPosition.y, _chip.transform.localPosition.z);
                }
            }
        }
    }

    private void OnDragStart()
    {
        m_mousePosition = InputManager.Position;
        InputManager.Event_Position += OnMove;
    }

    private void OnDragCancel()
    {
        InputManager.Event_Position -= OnMove;
        CameraManager.Singleton.SetFreeLookCamIsMove(false);
    }

    private void OnMove()
    {
        if ((m_mousePosition - InputManager.Position).magnitude >= 20.0f)
        {
            CameraManager.Singleton.SetFreeLookCamIsMove(true);
            InputManager.Event_Position -= OnMove;
        }
    }

    private void OnButton_Place()
    {
        MapManager.Singleton.Select.enabled = false;
    }
}