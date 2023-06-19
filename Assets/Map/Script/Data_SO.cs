using System;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "Create_Map_SO")]
public class Data_SO : ScriptableObject
{
    public int x = 3;
    public int y = 3;
    public int[] mapChip;
    public int[] objChip;

    public Data_SO()
    {
        mapChip = new int[y * x];
        objChip = new int[y * x];
    }
}