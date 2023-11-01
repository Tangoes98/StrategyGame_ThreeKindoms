using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAttackAction : BaseAttackAction
{
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Update()
    {
        base.Update();
        if (!_isActive) return;
    }

    public override bool IsEnabled() => _isActionEnabled;
    public override string GetActionName() => _actionName;
    public override int GetActionCost() => _actionCost;



    public override void TakeAction(GridPosition gridPosition, Action onActionCompleted)
    {
        _isActive = true;
        this._onActionCompleted = onActionCompleted;


        if (LevelGrid.Instance.HasUnitOnGridPosition(gridPosition))
        {
            DealDamageToTargetUnit(gridPosition);
        }

        if (LevelGrid.Instance.HasConstructionOnGridPosition(gridPosition))
            DealDamageToTargetConstruction(gridPosition);

        ActionCompleted();
    }



    void DealDamageToTargetUnit(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        HealthSystem targetHealthSystem = targetUnit.GetComponent<HealthSystem>();
        targetHealthSystem.OnDamage(_damageAmount);
    }

    void DealDamageToTargetConstruction(GridPosition gridPosition)
    {
        Construction targetConstruction = LevelGrid.Instance.GetConstructionAtGridPosition(gridPosition);
        ConstructionHealthSystem targetHealthSystem = targetConstruction.GetComponent<ConstructionHealthSystem>();
        targetHealthSystem.OnDamage(_damageAmount);

    }


    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetUnitGridPosition();

        ShowActionGridPositionRange(unitGridPosition, _attackRange, _excludedAttackRange, _unitType);

        for (int x = -_attackRange; x <= _attackRange; x++)
        {
            for (int z = -_attackRange; z <= _attackRange; z++)
            {
                int moveDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (moveDistance > _attackRange) continue;

                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition avaliableGridPosition = unitGridPosition + offsetGridPosition;

                // gridposition in the unit system
                if (!LevelGrid.Instance.IsValidGridPosition(avaliableGridPosition)) continue;

                // gridposition is not unit self gridposition
                if (avaliableGridPosition == unitGridPosition) continue;

                // gridposition has no unit on it
                if (!LevelGrid.Instance.HasUnitOnGridPosition(avaliableGridPosition)
                    && !LevelGrid.Instance.HasConstructionOnGridPosition(avaliableGridPosition)) continue;

                // Check if the excluded girdposition list includes this gridposition
                if (_excludedGridPosition.Contains(avaliableGridPosition)) continue;

                // check if both are enemy or friendly unit
                // Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(avaliableGridPosition);
                // if (targetUnit.IsEnemyUnit() == _unit.IsEnemyUnit()) continue;

                // Check if there is a Friendly or Enemy construction on the grid position
                // if (!LevelGrid.Instance.GetGridObject(avaliableGridPosition).HasConstructionOnGrid()) continue;


                validGridPositionList.Add(avaliableGridPosition);

            }
        }
        return validGridPositionList;
    }


}
