using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float HSVColor { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public uint BotOperations { get; private set; }
    [field: SerializeField] public List<BotSetting> BotSettings { get; private set; }

    public void Initialize(int index_, string name_, float hsv_, int bots_, int botMax_)
    {
        Index = index_;
        Name = name_;
        HSVColor = hsv_;
        Color = Color.HSVToRGB(hsv_, 1.0f, 1.0f);

        var _operations = (uint)bots_;
        BotOperations = _operations;
        BotSettings = new(botMax_);
        for (int i = 0; i < _operations; ++i)
        {
            var _setting = gameObject.AddComponent<BotSetting>();
            _setting.Initialize(index_, 10.0f, 10.0f, 1.0f);
            BotSettings.Add(_setting);
        }
    }
    public void SetName(string name_)
    {
        Name = name_;
    }
}