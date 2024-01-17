using UnityEngine;

[CreateAssetMenu(menuName = "BoB/Card/Category")]
public class Category_SO : ScriptableObject
{
    public int ID;
    public string Name;
    public Color Color;
    public Color Color_BG;
    public Color Color_Frame;
    public Color Color_Icon;
    public Color Color_Text;
    public Sprite Icon;
    public Sprite Image;
}