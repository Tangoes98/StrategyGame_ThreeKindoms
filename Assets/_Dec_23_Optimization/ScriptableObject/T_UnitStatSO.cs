using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/CreatNewUnitStatSO", order = 1)]
public class T_UnitStatSO : ScriptableObject
{
    // !Should never modify variables during playmode! // 
    [Header("Basic Unit Stat")]
    public string Name;
    public float AttackValue;
    public float DefenceValue;
    public float TroopValue;
}
