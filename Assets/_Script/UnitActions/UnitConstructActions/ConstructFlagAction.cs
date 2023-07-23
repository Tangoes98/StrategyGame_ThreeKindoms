using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructFlagAction : UnitBaseConstructAction
{
    [Header("Action_Information")]
    [SerializeField] Transform _flagConstructionPrefab;

    Vector3 _targetPosition;



    public override int GetActionCost() => _actionCost;
    public override string GetActionName() => _actionName;
    public override bool IsEnabled() => _isActionEnabled;


    protected override void Update()
    {
        base.Update();
        if (!_isActive) return;

        BuildFlag(_targetPosition);
        Debug.Log("BuildFlagComplete");

        ActionCompleted();

    }

    public override void TakeAction(GridPosition mouseGridPosition, Action onActionCompleted)
    {
        _isActive = true;

        _onActionCompleted = onActionCompleted;

        if (_useCount < 1) return;

        //if (!IsValidActionGridPosition(mouseGridPosition)) return;

        SetActionWorldPosition(mouseGridPosition);

        _useCount -= 1;

    }

    Vector3 SetActionWorldPosition(GridPosition gridPosition) => _targetPosition = LevelGrid.Instance.GetWorldPositionWithHeight(gridPosition);
    void BuildFlag(Vector3 worldPosition)
    {
        Transform flagTransform = Instantiate(_flagConstructionPrefab, worldPosition, Quaternion.identity);
        Construction flag = flagTransform.GetComponent<Construction>();

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(worldPosition);
        LevelGrid.Instance.AddConstructionToGrdObject(gridPosition, flag);
    }






    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> actionGridPositionList = new List<GridPosition>();

        for (int x = -_actionRange; x <= _actionRange; x++)
        {
            for (int z = -_actionRange; z <= _actionRange; z++)
            {
                int distance = Mathf.Abs(x) + Mathf.Abs(z);
                if (distance > _actionRange) continue;

                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition tempValidGridposition = offsetGridPosition + _unitGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(tempValidGridposition)) continue;

                // Skip if gridposition is Water
                PathNode pathNode = Pathfinding.Instance.GetNode(tempValidGridposition);
                var terrain = pathNode.GetTerrain().GetTerrainType();
                if (terrain == TerrainType.TerrainTypeEnum.StreamShallow ||
                terrain == TerrainType.TerrainTypeEnum.StreamDeep) continue;

                // Skip if gridposition is too high
                if (!Pathfinding.Instance.CompareFloorHeight(_unitGridPosition, tempValidGridposition)) continue;

                // Skip if gridposition already contains a Construction
                if (LevelGrid.Instance.HasConstructionOnGridPosition(tempValidGridposition)) continue;

                actionGridPositionList.Add(tempValidGridposition);
            }
        }

        if (_useCount < 1)
        {
            List<GridPosition> enmptyGridPositionlist = new List<GridPosition>();
            return enmptyGridPositionlist;
        }
        else return actionGridPositionList;


    }
}
