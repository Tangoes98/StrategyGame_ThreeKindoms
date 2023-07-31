using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructRoadAction : UnitBaseConstructAction
{
    [Header("Action_Road")]
    [SerializeField] Transform _roadConstructionPrefab;



    public override int GetActionCost() => _actionCost;
    public override string GetActionName() => _actionName;
    public override bool IsEnabled() => _isActionEnabled;


    protected override void Update()
    {
        base.Update();
        if (!_isActive) return;

        BuildRoad(_targetPosition);

        Debug.Log("BuildRoad");

        ActionCompleted();

    }

    public override void TakeAction(GridPosition mouseGridPosition, Action onActionCompleted)
    {
        _isActive = true;

        _onActionCompleted = onActionCompleted;

        Pathfinding.Instance.UpdatingGridMoveCost(mouseGridPosition);

        SetActionWorldPosition(mouseGridPosition);


    }

    Vector3 SetActionWorldPosition(GridPosition gridPosition) => _targetPosition = LevelGrid.Instance.GetWorldPositionWithHeight(gridPosition);
    void BuildRoad(Vector3 worldPosition)
    {
        Transform roadTransform = Instantiate(_roadConstructionPrefab, worldPosition, Quaternion.identity);
        Construction road = roadTransform.GetComponent<Construction>();

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(worldPosition);
        LevelGrid.Instance.AddConstructionToGrdObject(gridPosition, road);
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

                //skip if not included in gird system
                if (!LevelGrid.Instance.IsValidGridPosition(tempValidGridposition)) continue;

                //skip if unit is over grid position
                if (tempValidGridposition == _unitGridPosition) continue;

                // Skip if gridposition is Water
                PathNode pathNode = Pathfinding.Instance.GetNode(tempValidGridposition);
                var terrain = pathNode.GetTerrain().GetTerrainType();
                if (
                    terrain == TerrainType.TerrainTypeEnum.StreamShallow ||
                    terrain == TerrainType.TerrainTypeEnum.StreamDeep) continue;

                // Skip if gridposition is too high
                if (!Pathfinding.Instance.CanClimbNeighbourGrid(_unitGridPosition, tempValidGridposition)) continue;

                // Skip if gridposition already contains a Construction
                if (LevelGrid.Instance.HasConstructionOnGridPosition(tempValidGridposition)) continue;

                actionGridPositionList.Add(tempValidGridposition);
            }
        }
        return actionGridPositionList;
    }



}
