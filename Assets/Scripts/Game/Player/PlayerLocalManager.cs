using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocalManager : MonoBehaviour
{
    [SerializeField] MapChip m_SelectChip;

    void OnEnable()
    {
        PlayerInputManager.OnMouseMainClickEvent += OnMouse_MainClick;
    }
    void OnDisable()
    {
        PlayerInputManager.OnMouseMainClickEvent -= OnMouse_MainClick;
    }

    void OnMouse_MainClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(PlayerInputManager.m_Pos);

        Debug.Log(PlayerInputManager.m_Pos);
        int mask = 1 << Name.Layer.Map;
        if(Physics.Raycast(ray, out var hit, Mathf.Infinity, mask))
        {
            var map = hit.collider.GetComponent<MapChip>();
            if(m_SelectChip != map)
            {
                m_SelectChip?.Stop();
                map.HighLight();
                m_SelectChip = map;

            }
        }
    }
}