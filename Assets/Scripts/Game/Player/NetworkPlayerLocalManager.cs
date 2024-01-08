using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map.Chip;

public class NetworkPlayerLocalManager : MonoBehaviour
{
    public MapChip m_SelectChip { get; private set; }
    Vector2 m_Position;

    void OnEnable()
    {
        InputManager.Event_Main += OnMouse_MainClick;
        InputManager.Event_DragStart += OnMouse_DragStart;
        InputManager.Event_DragCancel += OnMouse_DragCancel;
    }
    void OnDisable()
    {
        InputManager.Event_Main -= OnMouse_MainClick;
        InputManager.Event_DragStart -= OnMouse_DragStart;
        InputManager.Event_DragCancel -= OnMouse_DragCancel;
    }

    void OnMouse_MainClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Position);

        int mask = 1 << Name.Layer.Map | 1 << Name.Layer.UI;
        if(Physics.Raycast(ray, out var hit, Mathf.Infinity, mask))
        {
            var map = hit.collider.GetComponent<MapChip>();
            if(m_SelectChip != map)
            {
                m_SelectChip = map;
            }
        }
    }

    void OnMouse_DragStart()
    {
        m_Position = InputManager.Position;
        InputManager.Event_Position += OnMouse_MovePerform;
    }

    void OnMouse_DragCancel()
    {
        InputManager.Event_Position -= OnMouse_MovePerform;
        CameraManager.Singleton.SetFreeLookCamIsMove(false);
    }

    void OnMouse_MovePerform()
    {
        if ((m_Position - InputManager.Position).magnitude >= 20.0f)
        {
            CameraManager.Singleton.SetFreeLookCamIsMove(true);
            InputManager.Event_Position -= OnMouse_MovePerform;
        }
    }
}