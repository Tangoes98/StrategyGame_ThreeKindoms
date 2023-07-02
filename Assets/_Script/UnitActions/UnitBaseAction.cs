using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UnitBaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected bool _isActive;
    protected Action _onActionCompleted;
    protected GridPosition _unitGridPosition;

    protected virtual void Awake()
    {
        _unit = transform.GetComponent<Unit>();
    }
    protected virtual void Update()
    {
        _unitGridPosition = _unit.GetUnitGridPosition();
    }

    public abstract void TakeAction(GridPosition gridPos, Action onActionCompleted);

    public abstract string GetActionName();

    public abstract int GetActionCost();

    public virtual bool IsValidActionGridPosition(GridPosition gridpos)
    {
        List<GridPosition> validGridPositionList = GetValidGridPositionList();

        return validGridPositionList.Contains(gridpos);
    }

    public abstract List<GridPosition> GetValidGridPositionList();

    protected void ActionCompleted()
    {
        _isActive = false;
        _onActionCompleted();
    }

}
