using System;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "Create_Map_SO")]
public class MapData_SO : ScriptableObject
{
    //Vector2Intで実装しない理由は ???.Size.y や .xでアクセスするのは
    //サイズとして明示的で他のデータを持たせる場合はとてもいいが、
    //今回に限りデータ量が少ないのと .Size. を毎回挟むのが記述量が多くなって嫌なので
    public int x = 3;
    public int y = 3;
    public int[] mapChip;
    public int[] objChip;

    public MapData_SO()
    {
        mapChip = new int[y * x];
        objChip = new int[y * x];
    }
}