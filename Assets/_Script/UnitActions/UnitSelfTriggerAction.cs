using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitSelfTriggerAction : UnitBaseAction
{
    Vector3 _position;

    [SerializeField] int _actionCost;

    protected override void Awake()
    {
        base.Awake();
        _position = transform.position;

    }

    protected override void Update()
    {
        base.Update();
        if (!_isActive) return;
    }

    public override int GetActionCost()
    {
        return _actionCost;
    }

    public override void TakeAction(GridPosition gridPos, Action onActionCompleted)
    {
        Debug.Log("Self Spined");
        _isActive = true;
        this._onActionCompleted = onActionCompleted;

        ActionCompleted();
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPos = _unit.GetUnitGridPosition();

        return new List<GridPosition>
        {
            unitGridPos
        };
    }


    void TestingFuction()
    {
        // self spin 

        Debug.Log("Self Spined");
    }

    public override string GetActionName() => "Test_Special";
}
