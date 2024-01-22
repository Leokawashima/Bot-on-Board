using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float HSVColor { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public uint BotOperations { get; private set; }
    [field: SerializeField] public BotSetting[] BotSettings { get; private set; }

    public void Initialize(int index_, string name_, float hsv_)
    {
        Index = index_;
        Name = name_;
        HSVColor = hsv_;
        Color = Color.HSVToRGB(hsv_, 1.0f, 1.0f);
    }
}