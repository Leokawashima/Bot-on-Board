using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Table/MO_Table_SO")]
public class MapObjectTable_SO : ScriptableObject
{
    public MapObject_SO_Template[] m_Table;
}