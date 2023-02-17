using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComboArray
{
    [field: SerializeField] public AttackAnimData[] lightCombo;
    [field: SerializeField] public AttackAnimData[] heavyCombo;
}
