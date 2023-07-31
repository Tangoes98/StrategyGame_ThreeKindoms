using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConstructObstacleAction : UnitBaseConstructAction
{
    [Header("Action_Obstacle")]
    [SerializeField] Transform _obstacleConstructionPrefab;



    public override int GetActionCost() => _actionCost;
    public override string GetActionName() => _actionName;
    public override bool IsEnabled() => _isActionEnabled;


    protected override void Update()
    {
        base.Update();
        if (!_isActive) return;

        BuildObstacle(_targetPosition);

        Debug.Log("BuildObstacle");

        ActionCompleted();

    }

    public override void TakeAction(GridPosition mouseGridPosition, Action onActionCompleted)
    {
        _isActive = true;

        _onActionCompleted = onActionCompleted;

        SetActionWorldPosition(mouseGridPosition);

    }

    Vector3 SetActionWorldPosition(GridPosition gridPosition) => _targetPosition = LevelGrid.Instance.GetWorldPositionWithHeight(gridPosition);
    void BuildObstacle(Vector3 worldPosition)
    {
        Transform obstacleTransform = Instantiate(_obstacleConstructionPrefab, worldPosition, Quaternion.identity);
        Construction obstacle = obstacleTransform.GetComponent<Construction>();

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(worldPosition);
        LevelGrid.Instance.AddConstructionToGrdObject(gridPosition, obstacle);
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
