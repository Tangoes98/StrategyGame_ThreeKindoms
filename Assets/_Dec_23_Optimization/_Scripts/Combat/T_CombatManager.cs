using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_CombatManager : MonoBehaviour
{
    #region =========== Singleton ================
    public static T_CombatManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;
    }
    #endregion

    #region ============= Variables =============



    #endregion

    #region ======== Public getters and setters =========

    public float G_GetHeightDamageMultiplier(int value) => HeightDamageMultiplier(value);
    public float G_GetMoraleDamageMultiplier(int value) => MoraleDamageMultiplier(value);
    public float G_DealDamage(T_Unit a, T_Unit b, out float h, out float m) => TotalDamageCalculation(a, b, out h, out m);

    #endregion






    #region ============ Methods =============

    #region ====== Damage ======

    /// <summary>
    /// a for player, b for opponent
    /// </summary>
    float TotalDamageCalculation(T_Unit a, T_Unit b, out float h, out float m)
    {
        T_UnitStatSO aStat = a.G_GetUnitStatSO();
        T_UnitStatSO bStat = b.G_GetUnitStatSO();


        float attackValue = aStat.AttackValue;
        float totalAttackMultiplier = TotalAttackMultiplier(a, b, out float heightMulti, out float moraleMulti);
        float enemyDefenceValue = bStat.DefenceValue;
        float enemyDefenceBonus = 0f;
        float armorBreakingDamageValue = 0f;

        h = heightMulti;
        m = moraleMulti;

        float finalDamageValue =
        Mathf.Pow(attackValue * totalAttackMultiplier, 2f) /
            ((attackValue * totalAttackMultiplier) + (enemyDefenceValue + enemyDefenceBonus))
            + armorBreakingDamageValue;

        return finalDamageValue;
    }



    #endregion

    #region ====== Multiplier ======
    /// <summary>
    /// a for player, b for opponent
    /// </summary>
    float TotalAttackMultiplier(T_Unit a, T_Unit b, out float h, out float m)
    {
        float unitTypeDamageMultiplier = 0f;

        int a_floor = a.G_GetUnitCurrentFloorHeight();
        int b_floor = b.G_GetUnitCurrentFloorHeight();
        float heightDamageMultiplier = HeightDamageMultiplier(a_floor - b_floor);
        h = heightDamageMultiplier;

        float unitTypeEffectivenessBonus = 0f;

        float unitActiveSkillBonusMultiplier = 0f;

        float moraleBonus = MoraleDamageMultiplier(T_Morale.Instance.G_GetCurrentMorale());
        m = moraleBonus;

        float totalAttackMultiplier =
            unitTypeDamageMultiplier +
            heightDamageMultiplier +
            unitTypeEffectivenessBonus +
            unitActiveSkillBonusMultiplier +
            moraleBonus;

        return totalAttackMultiplier;
    }

    float HeightDamageMultiplier(int height)
    {
        if (Between(height, -5f, -3f)) return -0.2f;
        else if (Between(height, -2f, 2f)) return 0f;
        else if (Between(height, 3f, 5f)) return 0.2f;
        else
        {
            Debug.LogError("Invalid height");
            return 0f;
        }
    }

    float MoraleDamageMultiplier(int morale)
    {
        if (Between(morale, 0f, 2f)) return -0.2f;
        else if (Between(morale, 3f, 7f)) return 0f;
        else if (Between(morale, 8f, 10f)) return 0.2f;
        {
            Debug.LogError("Invalid morale");
            return 0f;
        }
    }



    /// <summary>
    /// x is the input value, a is minimum, b is maximum
    /// </summary>
    bool Between(float x, float a, float b)
    {
        if (a <= x && x <= b) return true;
        else return false;
    }

    #endregion

    #region ======== ??? ==========





    #endregion

    #endregion







}
