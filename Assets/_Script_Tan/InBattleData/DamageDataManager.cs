using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDataManager : MonoBehaviour
{

    // Unit basic attributes
    int attackValue;
    int defenceValue;
    int enemyDefenceValue; // enemy
    int troopsValue;
    int currentTroops;
    int unitMovementLimit;
    int currentUnitMovementLimit;
    float unitDamageMultiplier;
    float unitDefenceMultiplier;
    float enemyUnitDefenceMultiplier; // enemy
    int unitMorale;
    int currentUnitMorale;


    // Critical hit bonus
    float criticalHitRate;
    bool isCriticalHit;
    float criticalHitValue;


    // Height difference bonus
    float heightBuff;
    int heightDifference;
    float heightDamageMultiplier;


    // Unit types advantage bonus
    bool isUnitTypeAdvantage;
    int unitTypeAdvantageBonus;
    float unitTypeAdvantageDamageMultiplier;
    float unitTypeAdvantageDefenceMultiplier;
    float enemyUnitTypeAdvantageDefenceMultiplier; // enemy


    // Passive/Active Skills bonus
    float activeSkillMultiplier;
    float passiveSkillMultiplier;
    int defenceBonus;
    int enemyDefenceBonus; // enemy


    // Morale bonus
    float moraleBonus;


    // Total Damage bonus
    float totalDamageMultiplier;


    // Armor breaking damage value
    float armorBreakingDamageValue;


    float totalDamageValue;
    float totalCriticalHitDamageValue;

    float TotalDamageCalculate()
    {
        float _attackValue = (float)attackValue;
        float _defenceValue = (float)defenceValue;
        float _troopsValue = (float)troopsValue;
        float _currentTroops = (float)currentTroops;
        float _unitMovementLimit = (float)unitMovementLimit;
        float _currentUnitMovementLimit = (float)currentUnitMovementLimit;
        float _unitMorale = (float)unitMorale;
        float _currentUnitMorale = (float)currentUnitMorale;
        float _heightDifference = (float)heightDifference;
        float _unitTypeAdvantageBonus = (float)unitTypeAdvantageBonus;
        float _defenceBonus = (float)defenceBonus;


        float totalAttackValue = attackValue * unitDamageMultiplier;

        totalDamageValue =
            Mathf.Pow((totalAttackValue * totalDamageMultiplier), 2f) /
            (((totalAttackValue * totalDamageMultiplier) + (enemyDefenceValue + enemyDefenceBonus))
            * (enemyUnitDefenceMultiplier + enemyUnitTypeAdvantageDefenceMultiplier)
            + armorBreakingDamageValue);

        return totalDamageValue;
    }



    // Wisdom damage
    float wisdomDamageValue;

}
