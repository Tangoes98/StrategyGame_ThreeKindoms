using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAttackAction : UnitBaseAction
{
    Vector3 _position;
    [SerializeField] int _actionCost;
    [SerializeField] int _attackRange;
    [SerializeField] int _damageAmount;

    protected override void Awake()
    {
        base.Awake();
        _position = transform.position;

    }

    void Update()
    {
        if (!_isActive) return;


    }


    public override string GetActionName() => "Test_Attack";
    public override int GetActionCost() => _actionCost;



    public override void TakeAction(GridPosition gridPosition, Action onActionCompleted)
    {
        _isActive = true;
        this._onActionCompleted = onActionCompleted;

        DealDamageToTargetUnit(gridPosition);

        ActionCompleted();
    }

    void DealDamageToTargetUnit(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        HealthSystem targetHealthSystem = targetUnit.GetComponent<HealthSystem>();
        targetHealthSystem.OnDamage(_damageAmount);
    }


    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetUnitGridPosition();

        GridSystemVisual.Instance.ShowGridPositionRange(unitGridPosition, _attackRange);

        for (int x = -_attackRange; x <= _attackRange; x++)
        {
            for (int z = -_attackRange; z <= _attackRange; z++)
            {
                int moveDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (moveDistance > _attackRange) continue;

                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition avaliableGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(avaliableGridPosition)) continue; // gridposition in the unit system

                if (avaliableGridPosition == unitGridPosition) continue; // gridposition is not unit self gridposition

                if (!LevelGrid.Instance.HasUnitOnGridPosition(avaliableGridPosition)) continue; // gridposition has no unit on it

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(avaliableGridPosition);
                if (targetUnit.IsEnemyUnit() == _unit.IsEnemyUnit()) continue; // check if both are enemy or friendly unit


                validGridPositionList.Add(avaliableGridPosition);

            }
        }
        return validGridPositionList;
    }


}
