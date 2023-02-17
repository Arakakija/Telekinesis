using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttackType
{
    Heavy,
    Light,
}

[CreateAssetMenu(fileName = "New AttackAnim Data", menuName = "AnimationData/Attacks")]
public class AttackAnimData : ScriptableObject
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float  TransitionDuration { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
    [field: SerializeField] public float ComboAttackTime { get; private set; }
    [field: SerializeField] public float ForceTime { get; private set; }
    [field: SerializeField] public float Force { get; private set; }
    [field: SerializeField] public AttackType Type { get; private set; }
}
