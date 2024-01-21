using System;
using UnityEngine;
using TMPro;

public class PlusMinusButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [field: SerializeField] public int Value { get; private set; } = 0;
    [field: SerializeField] public int ValueMin { get; private set; } = 0;
    [field: SerializeField] public int ValueMax { get; private set; } = 4;

    public event Action<int> Event_ValueChanged;

    public void Increment()
    {
        SetValue(Value + 1);
    }
    public void Decrement()
    {
        SetValue(Value - 1);
    }

    public int SetValue(int value_)
    {
        if (value_ > ValueMax)
        {
            Value = ValueMax;
            Event_ValueChanged?.Invoke(Value);
            m_text.text = Value.ToString();
            return Value;
        }
        if (value_ < ValueMin)
        {
            Value = ValueMin;
            Event_ValueChanged?.Invoke(Value);
            m_text.text = Value.ToString();
            return Value;
        }

        Value = value_;
        Event_ValueChanged?.Invoke(Value);
        m_text.text = Value.ToString();
        return Value;
    }
    public void SetMin(int min_)
    {
        ValueMin = min_;
        SetValue(Value);
    }
    public void SetMax(int max_)
    {
        ValueMax = max_;
        SetValue(Value);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SetValue(Value);
    }
#endif
}