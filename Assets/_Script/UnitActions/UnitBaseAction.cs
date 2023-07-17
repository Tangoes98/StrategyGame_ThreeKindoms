using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UnitBaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected Vector3 _unitTransformPosition;
    protected bool _isActive;
    protected Action _onActionCompleted;
    protected GridPosition _unitGridPosition;

    public abstract bool IsEnabled();

    [SerializeField] protected bool _isActionEnabled;
    //[SerializeField] protected int _actionCost;


    protected virtual void Awake()
    {
        _unit = transform.GetComponent<Unit>();

    }
    protected virtual void Update()
    {
        _unitTransformPosition = _unit.transform.position;
        _unitGridPosition = _unit.GetUnitGridPosition();
    }



    public virtual bool IsValidActionGridPosition(GridPosition gridpos)
    {
        List<GridPosition> validGridPositionList = GetValidGridPositionList();

        return validGridPositionList.Contains(gridpos);
    }



    public abstract void TakeAction(GridPosition mouseGridPosition, Action onActionCompleted);

    public abstract string GetActionName();

    public abstract int GetActionCost();

    public abstract List<GridPosition> GetValidGridPositionList();




    protected void ActionCompleted()
    {
        _isActive = false;
        _onActionCompleted();
    }

}
