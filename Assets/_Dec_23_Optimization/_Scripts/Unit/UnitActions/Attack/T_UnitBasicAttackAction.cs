using System;
using System.Collections;
using System.Collections.Generic;
using DebugConsole;
using UnityEngine;

public class T_UnitBasicAttackAction : T_UnitActionBase
{
    [Header("LOCAL_VARIABLES")]
    [SerializeField] int _attackRange;
    T_LevelGridManager _levelGridInstance;
    T_Unit _OpponentUnit;

    [Header("DEBUG_VIEW")]
    [SerializeField] List<T_GirdPosition> _opponentUnitGridPos;
    [SerializeField] int _damageValue;





    protected override void Start()
    {
        base.Start();
        _levelGridInstance = T_LevelGridManager.Instance;
    }
    protected override void Update()
    {
        base.Update();
        if (!P_isActive) return;

        switch (P_actionState)
        {
            case Action_State.Action_Selection:




                break;

            case Action_State.Action_Preview:

                CancelSelectedActionCheck();
                if (!CheckActionInput(out T_GirdPosition targetGridPosition)) return;
                if (!CheckSelectedAttackGridPosition(targetGridPosition)) return;
                P_actionState = Action_State.Action_Busy;

                break;

            case Action_State.Action_Busy:
                SetAllActionButtonState(false);
                TakeAction();

                break;

            case Action_State.Action_Completed:
                SetSingleActionButtonState(true);
                T_LevelGridManager.Instance.G_ClearAllGridValidationVisuals();
                Debug.Log("ATTACK_COMPLETE");
                P_isActive = false;

                break;

        }
    }

    #region ============ Parent Abstract Function Implementation =========

    protected override void TakeAction()
    {
        Debug.Log("ACTION START: " + P_actionName);
        DealDamageToOpponentUnit();

    }
    protected override void PreviewActionValidPosition()
    {
        AttackRangePreview();

    }

    #endregion ================================================
    void AttackRangePreview()
    {
        _levelGridInstance.G_ShowGridValidationVisuals("ATTACK_RANGE", ValidGridPositionList());
        _levelGridInstance.G_ShowGridValidationVisuals("VALID_ATTACK", EnemyUnitGridValidation());
    }

    List<T_GirdPosition> ValidGridPositionList()
    {
        List<T_GirdPosition> gridPosList = new();
        T_GirdPosition unitGp = T_LevelGridManager.Instance.G_WorldToGridPosition(_unit.transform.position);
        for (int x = -_attackRange; x <= _attackRange; x++)
        {
            for (int z = -_attackRange; z <= _attackRange; z++)
            {

                int tempDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (tempDistance > _attackRange) continue;


                T_GirdPosition offsetGridPosition = new T_GirdPosition(x, z);
                T_GirdPosition ValidGridposition = offsetGridPosition + unitGp;


                if (!_levelGridInstance.G_IsValidSystemGrid(ValidGridposition)) continue;

                // Check if the position has enemy unit
                if (_levelGridInstance.G_GetGridPosData(ValidGridposition).G_GetUnitList().Count > 0)
                    _opponentUnitGridPos.Add(ValidGridposition);


                gridPosList.Add(ValidGridposition);
            }
        }
        return gridPosList;
    }

    List<T_GirdPosition> EnemyUnitGridValidation()
    {
        //TODO: Check if the gridPosition contains enemy unit

        List<T_GirdPosition> gridPosList = new();
        foreach (var gridPos in _opponentUnitGridPos)
        {
            if (gridPos == _unit.G_UnitGridPosition()) continue;

            gridPosList.Add(gridPos);
        }
        return gridPosList;
    }

    bool CheckSelectedAttackGridPosition(T_GirdPosition gridPos)
    {
        if (!EnemyUnitGridValidation().Contains(gridPos)) return false;
        else
        {
            _OpponentUnit = _levelGridInstance.G_GetGridPosData(gridPos).G_GetUnitList()[0];
            return true;
        }
    }

    void DealDamageToOpponentUnit()
    {
        float damageValue = T_CombatManager.Instance.G_DealDamage(_unit, _OpponentUnit, out float heightMulti, out float moraleMulti);
        int intDamage = Mathf.RoundToInt(damageValue);
        _OpponentUnit.G_GetHealthSystem().OnDamage(intDamage);
        P_actionState = Action_State.Action_Completed;

        // log Attacking action to console
        T_GameConsole.Instance.G_ConsoleLogAttack(_unit, _OpponentUnit, intDamage, heightMulti, moraleMulti);
    }








}
