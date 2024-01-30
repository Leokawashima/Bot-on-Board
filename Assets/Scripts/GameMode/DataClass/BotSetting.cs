﻿using System;
using UnityEngine;

[Serializable]
public class BotSetting
{
    [field: SerializeField] public int Index { get; private set; } = -1;
    [field: SerializeField] public float HP { get; private set; }
    [field: SerializeField] public float HPMax { get; private set; }
    [field: SerializeField] public float Attack { get; private set; }

    public void Initialize(int index_, float hp_, float hpMax_, float attack_)
    {
        Index = index_;
        HP = hp_;
        HPMax = hpMax_;
        Attack = attack_;
    }
}