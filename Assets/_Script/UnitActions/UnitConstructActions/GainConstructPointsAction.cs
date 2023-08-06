using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainConstructPointsAction : UnitBaseConstructAction
{
    [Header("GainConstructPointsAction")]
    [SerializeField] int _constructionPoints;



    public override int GetActionCost() => _actionCost;
    public override string GetActionName() => _actionName;
    public override bool IsEnabled() => _isActionEnabled;
    public override int GetConstructionActionCost() => _constructCost;
    public override bool IsSpendingConstructionCost() => _isSpendingConstructionCost;

    protected override void Update()
    {
        base.Update();
        if (!_isActive) return;

        Debug.Log("GainConstructionPoints");

        ActionCompleted();

    }



    public override void TakeAction(GridPosition mouseGridPosition, Action onActionCompleted)
    {
        _isActive = true;

        _onActionCompleted = onActionCompleted;

        if (_useLimit < 1) return;

        _useLimit -= 1;

        Unit unit = LevelGrid.Instance.GetUnitAtGridPosition(mouseGridPosition);
        unit.AddUnitConstructionPoints(_constructionPoints);

    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        validGridPositionList.Add(_unitGridPosition);

        if (_useLimit < 1)
        {
            List<GridPosition> emptyGridPositionList = new List<GridPosition>();
            return emptyGridPositionList;
        }
        else return validGridPositionList;
    }


}
