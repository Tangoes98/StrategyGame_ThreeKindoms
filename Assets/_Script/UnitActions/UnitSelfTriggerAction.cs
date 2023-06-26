using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitSelfTriggerAction : UnitBaseAction
{
    Vector3 _position;

    protected override void Awake()
    {
        base.Awake();
        _position = transform.position;

    }

    void Update()
    {

    }

    public override void TakeAction(GridPosition gridPos, Action onActionCompleted)
    {
        Debug.Log("Self Spined");
        this._onActionCompleted = onActionCompleted;
        //_onActionCompleted();
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
