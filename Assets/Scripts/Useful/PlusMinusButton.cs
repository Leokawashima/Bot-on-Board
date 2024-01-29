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
        Event_ValueSub;

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
        var _assign = value_;
        if (_assign > ValueMax)
        {
            _assign = ValueMax;
        }
        if (_assign < ValueMin)
        {
            _assign = ValueMin;
        }

        var _diff = _assign - Value;
        if (_diff > 0)
        {
            Event_ValueAdd?.Invoke(_diff);
        }
        if (_diff < 0)
        {
            Event_ValueSub?.Invoke(-_diff);
        }

        Value = _assign;
        Event_ValueChanged?.Invoke(_assign);
        m_text.text = _assign.ToString();
        return _assign;
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
    public void SetMinMax(int min_, int max_)
    {
        ValueMin = min_;
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