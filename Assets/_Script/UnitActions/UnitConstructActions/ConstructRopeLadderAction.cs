using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructRopeLadderAction : UnitBaseConstructAction
{
    [Header("Action_RopeLadder")]
    [SerializeField] Transform _ropeLadderPrefab;
    Vector3 _targetPosition;



    public override int GetActionCost() => _actionCost;
    public override string GetActionName() => _actionName;
    public override bool IsEnabled() => _isActionEnabled;


    protected override void Update()
    {
        base.Update();

        if (!_isActive) return;

        BuildRopeLadder(_targetPosition);

        Debug.Log("RopeLadder_Built");

        ActionCompleted();

    }
    public override void TakeAction(GridPosition mouseGridPosition, Action onActionCompleted)
    {
        _isActive = true;

        _onActionCompleted = onActionCompleted;


        SetActionWorldPosition(mouseGridPosition);

    }

    Vector3 SetActionWorldPosition(GridPosition gridPosition) => _targetPosition = LevelGrid.Instance.GetWorldPositionWithHeight(gridPosition);
    void BuildRopeLadder(Vector3 worldPosition)
    {
        Transform campTransform = Instantiate(_ropeLadderPrefab, worldPosition, Quaternion.identity);
        Construction ropeLadder = campTransform.GetComponent<Construction>();

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(worldPosition);
        LevelGrid.Instance.AddConstructionToGrdObject(gridPosition, ropeLadder);
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

                // Skip if gridposition already contains a Construction
                if (LevelGrid.Instance.HasConstructionOnGridPosition(tempValidGridposition)) continue;

                // Skip if gridposition is not high enough
                //if (Pathfinding.Instance.CompareFloorHeight(_unitGridPosition, tempValidGridposition)) continue;

                List<GridPosition> neighbourGridpositionList = GetNeighbourGridpositionList(tempValidGridposition);
                foreach (GridPosition neighbourGrid in neighbourGridpositionList)
                {
                    if (Pathfinding.Instance.CompareFloorHeight(tempValidGridposition, neighbourGrid)) continue;

                    actionGridPositionList.Add(tempValidGridposition);
                }
            }
        }

        return actionGridPositionList;
    }



    List<GridPosition> GetNeighbourGridpositionList(GridPosition currentGridPosition)
    {
        List<GridPosition> neighbourList = new List<GridPosition>();

        GridPosition rightGridpos = new GridPosition(currentGridPosition.x + 1, currentGridPosition.z);
        GridPosition leftGridpos = new GridPosition(currentGridPosition.x - 1, currentGridPosition.z);
        GridPosition upGridpos = new GridPosition(currentGridPosition.x, currentGridPosition.z + 1);
        GridPosition downGridpos = new GridPosition(currentGridPosition.x, currentGridPosition.z - 1);

        if (LevelGrid.Instance.IsValidGridPosition(rightGridpos)) neighbourList.Add(rightGridpos);
        if (LevelGrid.Instance.IsValidGridPosition(leftGridpos)) neighbourList.Add(leftGridpos);
        if (LevelGrid.Instance.IsValidGridPosition(upGridpos)) neighbourList.Add(upGridpos);
        if (LevelGrid.Instance.IsValidGridPosition(downGridpos)) neighbourList.Add(downGridpos);

        return neighbourList;
    }
}


