using UnityEngine;

[CreateAssetMenu(fileName = "CardImageTable", menuName = "BoB/Card/CardImageTable")]
public class CardImageTable_SO : ScriptableObject
{
    public MapObjectCard Prefab;

    public Sprite
        BG_UnCommon,
        BG_Rare;

    public Sprite
        Frame_Base,
        Frame_Epic;

    public Sprite[]
        Icon = new Sprite[5];

    public Sprite[]
        BG_Epic = new Sprite[5];
}