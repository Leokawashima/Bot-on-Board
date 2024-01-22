using System;
using UnityEngine;
using TMPro;

public class PlusMinusButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [field: SerializeField] public int Value { get; private set; } = 0;
    [field: SerializeField] public int ValueMin { get; private set; } = 0;
    [field: SerializeField] public int ValueMax { get; private set; } = 4;

    public event Action<int>
        Event_ValueChanged,
        Event_ValueAdd,
        Event_valueSub;

    public void Increment()
    {
        SetValue(Value + 1);
    }
    public void Decrement()
    {
        SetValue(Value - 1);
    }

    private int Set(int value_)
    {
        Value = value_;
        Event_ValueChanged?.Invoke(value_);
        m_text.text = value_.ToString();
        return value_;
    }

    public int SetValue(int value_)
    {
        if (value_ > ValueMax)
        {
            var _diff = ValueMax - Value;
            if (_diff > 0)
            {
                Event_ValueAdd?.Invoke(_diff);
            }
            return Set(ValueMax);
        }
        if (value_ < ValueMin)
        {
            var _diff = Value - ValueMin;
            if (_diff > 0)
            {
                Event_valueSub?.Invoke(_diff);
            }
            return Set(ValueMin);
        }
        {
            var _diff = Value - value_;
            if (_diff > 0)
            {
                Event_ValueAdd?.Invoke(_diff);
            }
            else
            {
                Event_valueSub?.Invoke(-_diff);
            }
            return Set(value_);
        }
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
}